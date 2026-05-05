using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Fractions.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Система принимает в себя CauseDamageRequest и уменьшает значение EnergyComp для целевой сущности
    /// </summary>
    public class CauseDamageSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<CauseDamageRequest>> _causeDamageRequestFilter = default;

        private readonly EcsPoolInject<CauseDamageRequest> _causeDamageRequestPool = default;
        private readonly EcsPoolInject<DeathRequest> _deathRequestPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        private readonly EcsPoolInject<FractionComp> _fractionPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _causeDamageRequestFilter.Value)
            {
                ref var causeDamageRequest = ref _causeDamageRequestPool.Value.Get(requestEntity);

                if (causeDamageRequest.To.Unpack(out var toWorld, out var targetEntity) == false)
                {
                    continue;
                }

                if (causeDamageRequest.From.Unpack(out var fromWorld, out var attackerEntity) == false)
                {
                    continue;
                }

                if (attackerEntity == targetEntity)
                {
                    continue;
                }

                if (_unitStatsPool.Value.Has(targetEntity) == false)
                {
                    continue;
                }
                
                if (_fractionPool.Value.Has(targetEntity) && _fractionPool.Value.Has(attackerEntity))
                {
                    ref var targetFraction = ref _fractionPool.Value.Get(targetEntity);
                    ref var attackerFraction = ref _fractionPool.Value.Get(attackerEntity);
                    
                    if (targetFraction.Fraction == attackerFraction.Fraction)
                    {
                        continue;
                    }
                }

                ref var targetStatsComp = ref _unitStatsPool.Value.Get(targetEntity);

                // Barriers
                if (targetStatsComp.BarrierCurrent > 0)
                {
                    targetStatsComp.BarrierCurrent -= 1;
                    causeDamageRequest.FinalDamage = 0;

                    return;
                }

                // Shields
                if (targetStatsComp.ShieldCurrent > 0)
                {
                    var shieldDamage = Mathf.Min(causeDamageRequest.FinalDamage, targetStatsComp.ShieldCurrent);
                    targetStatsComp.ShieldCurrent -= shieldDamage;
                    causeDamageRequest.FinalDamage -= shieldDamage;

                    if (causeDamageRequest.FinalDamage <= 0)
                    {
                        return;
                    }
                }
                
                // Armor
                causeDamageRequest.FinalDamage = Mathf.Max(0, causeDamageRequest.FinalDamage - targetStatsComp.Armor);
                
                // Energy
                var energyDamage = Mathf.Min(causeDamageRequest.FinalDamage, targetStatsComp.EnergyCurrent);
                targetStatsComp.EnergyCurrent -= energyDamage;
                causeDamageRequest.FinalDamage -= energyDamage;

                // Death
                if (targetStatsComp.EnergyCurrent <= 0)
                {
                    _deathRequestPool.Value.Add(_world.Value.NewEntity()).For = _world.Value.PackEntity(targetEntity);
                }
            }
        }
    }
}
