using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Переводит HitReactionRequest в состояние InHitReactionComp на цели.
    /// Конкретное перемещение/анимация реакции подключаются отдельными системами поведения.
    /// </summary>
    public class ApplyHitReactionRequestSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HitReactionRequest>> _hitReactionRequestFilter = default;

        private readonly EcsPoolInject<HitReactionRequest> _hitReactionRequestPool = default;
        private readonly EcsPoolInject<InHitReactionComp> _inHitReactionPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _hitReactionRequestFilter.Value)
            {
                ref var request = ref _hitReactionRequestPool.Value.Get(requestEntity);
                if (request.To.Unpack(out var world, out var targetEntity) == false)
                {
                    continue;
                }

                var attackerEntity = request.From.Unpack(out var fromWorld, out var unpackedAttackerEntity)
                    ? unpackedAttackerEntity
                    : -1;

                var duration = Mathf.Max(0.01f, request.Duration);
                var isAirKnockdown = request.Type == HitReactionFlags.Knockdown && IsGrounded(targetEntity) == false;

                ref var inHitReaction = ref _inHitReactionPool.Value.Has(targetEntity)
                    ? ref _inHitReactionPool.Value.Get(targetEntity)
                    : ref _inHitReactionPool.Value.Add(targetEntity);

                inHitReaction.Type = request.Type;
                inHitReaction.ReactionVelocity = request.ReactionVelocity;
                inHitReaction.WorldReactionVelocity = CalculateWorldReactionVelocity(attackerEntity, targetEntity, request.ReactionVelocity);
                inHitReaction.StartTime = Time.time;
                inHitReaction.Duration = duration;
                inHitReaction.WaitForGroundBeforeTimer = isAirKnockdown;
                inHitReaction.EndTime = isAirKnockdown ? float.PositiveInfinity : Time.time + duration;
            }
        }

        private Vector3 CalculateWorldReactionVelocity(int attackerEntity, int targetEntity, Vector2 reactionVelocity)
        {
            var direction = ResolveReactionPoint(targetEntity) - ResolveReactionPoint(attackerEntity);
            direction.y = 0f;

            if (direction.sqrMagnitude <= float.Epsilon)
            {
                direction = Vector3.forward;
            }

            return direction.normalized * reactionVelocity.x + Vector3.up * reactionVelocity.y;
        }

        private Vector3 ResolveReactionPoint(int entity)
        {
            if (entity >= 0 && _unitStatsPool.Value.Has(entity))
            {
                ref var stats = ref _unitStatsPool.Value.Get(entity);
                if (stats.ReactionCenter != null)
                {
                    return stats.ReactionCenter.position;
                }
            }

            if (entity >= 0 && _transformPool.Value.Has(entity))
            {
                ref var transformComp = ref _transformPool.Value.Get(entity);
                if (transformComp.Value != null)
                {
                    return transformComp.Value.position;
                }
            }

            return Vector3.zero;
        }

        private bool IsGrounded(int entity)
        {
            if (_characterControllerPool.Value.Has(entity) == false)
            {
                return true;
            }

            ref var characterController = ref _characterControllerPool.Value.Get(entity);
            return characterController.Value == null ||
                   characterController.Value.Motor == null ||
                   characterController.Value.Motor.GroundingStatus.IsStableOnGround;
        }
    }
}
