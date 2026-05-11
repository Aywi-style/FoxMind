using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Fractions.Components;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Списывает стабилизацию цели по успешному попаданию и создаёт запрос hit reaction, если стабилизация пробита.
    /// </summary>
    public class ApplyStabilizationDamageSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<CauseDamageRequest>> _damageRequestFilter = default;

        private readonly EcsPoolInject<CauseDamageRequest> _damageRequestPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        private readonly EcsPoolInject<FractionComp> _fractionPool = default;
        private readonly EcsPoolInject<HitReactionRequest> _hitReactionRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _damageRequestFilter.Value)
            {
                ref var damageRequest = ref _damageRequestPool.Value.Get(requestEntity);
                if (damageRequest.StabilizationDamage <= 0)
                {
                    continue;
                }

                if (damageRequest.From.Unpack(out var fromWorld, out var attackerEntity) == false)
                {
                    continue;
                }

                if (damageRequest.To.Unpack(out var toWorld, out var targetEntity) == false)
                {
                    continue;
                }

                if (attackerEntity == targetEntity || _unitStatsPool.Value.Has(targetEntity) == false)
                {
                    continue;
                }

                if (IsFriendlyFire(attackerEntity, targetEntity))
                {
                    continue;
                }

                ref var targetStats = ref _unitStatsPool.Value.Get(targetEntity);
                if (targetStats.StabilizationMax <= 0f)
                {
                    continue;
                }

                targetStats.StabilizationCurrent = Mathf.Max(0f, targetStats.StabilizationCurrent - damageRequest.StabilizationDamage);
                targetStats.LastStabilizationDamageTime = Time.time;

                if (damageRequest.HitReactionType == HitReactionType.None ||
                    targetStats.StabilizationCurrent > 0f ||
                    IsReactionAllowed(targetStats.AllowedHitReactions, damageRequest.HitReactionType) == false)
                {
                    continue;
                }

                ref var hitReactionRequest = ref _hitReactionRequestPool.Value.Add(_world.Value.NewEntity());
                hitReactionRequest.From = damageRequest.From;
                hitReactionRequest.To = damageRequest.To;
                hitReactionRequest.Type = damageRequest.HitReactionType;
                hitReactionRequest.ReactionVelocity = damageRequest.ReactionVelocity;
                hitReactionRequest.Duration = damageRequest.HitReactionDuration > 0f ? damageRequest.HitReactionDuration : 0.35f;
            }
        }

        private bool IsFriendlyFire(int attackerEntity, int targetEntity)
        {
            if (_fractionPool.Value.Has(targetEntity) == false || _fractionPool.Value.Has(attackerEntity) == false)
            {
                return false;
            }

            ref var targetFraction = ref _fractionPool.Value.Get(targetEntity);
            ref var attackerFraction = ref _fractionPool.Value.Get(attackerEntity);
            return targetFraction.Fraction == attackerFraction.Fraction;
        }

        private static bool IsReactionAllowed(HitReactionFlags allowedFlags, HitReactionType reactionType)
        {
            return reactionType switch
            {
                HitReactionType.StaggerAndAirJuggle => (allowedFlags & (HitReactionFlags.Stagger | HitReactionFlags.Juggle)) != 0,
                HitReactionType.Knockback => (allowedFlags & HitReactionFlags.Knockback) != 0,
                HitReactionType.Launch => (allowedFlags & HitReactionFlags.Launch) != 0,
                HitReactionType.Knockdown => (allowedFlags & HitReactionFlags.Knockdown) != 0,
                _ => false,
            };
        }
    }
}
