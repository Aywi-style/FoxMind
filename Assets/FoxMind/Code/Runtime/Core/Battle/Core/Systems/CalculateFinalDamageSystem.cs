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
    public class CalculateFinalDamageSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<CauseDamageRequest>> _causeDamageRequestFilter = default;

        private readonly EcsPoolInject<CauseDamageRequest> _causeDamageRequestPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _causeDamageRequestFilter.Value)
            {
                ref var causeDamageRequest = ref _causeDamageRequestPool.Value.Get(requestEntity);

                if (causeDamageRequest.From.Unpack(out var fromWorld, out var attackerEntity) == false)
                {
                    continue;
                }

                if (_unitStatsPool.Value.Has(attackerEntity) == false)
                {
                    continue;
                }
                
                ref var attackerStatsComp = ref _unitStatsPool.Value.Get(attackerEntity);

                var critChance = causeDamageRequest.BaseCritChance + (attackerStatsComp.Stabilization * 0.1f);
                var critMultiplier = causeDamageRequest.BaseCritMultiplier * (1 + 0.1f * attackerStatsComp.ClockSpeed);
                var critTier = Mathf.Floor(critChance);
                var tierRemainder = critChance - critTier;
                
                var roll = Random.Range(0f, 1f);
                if (roll < tierRemainder)
                {
                    critTier += 1;
                }

                var finalCritMultiplier = 1 + critTier * (critMultiplier - 1);

                causeDamageRequest.FinalDamage = Mathf.RoundToInt(finalCritMultiplier * causeDamageRequest.BaseDamage);
                Debug.Log($"FinalDamage: {causeDamageRequest.FinalDamage}");
            }
        }
    }
}