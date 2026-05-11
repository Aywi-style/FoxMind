using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class ProvideComboSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<TargetProvideComboRequest>> _targetProvideComboRequestFilter = default;

        private readonly EcsPoolInject<TargetProvideComboRequest> _targetProvideComboRequestPool = default;
        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<ProvideAttackRequest> _targetProvideAttackRequestPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_targetProvideComboRequestFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var targetProvideComboRequestEntity in _targetProvideComboRequestFilter.Value)
            {
                ref var targetProvideComboRequest = ref _targetProvideComboRequestPool.Value.Get(targetProvideComboRequestEntity);
                
                if (targetProvideComboRequest.PackedEntity.Unpack(_world.Value, out int targetEntity) == false)
                {
                    continue;
                }
                
                if (_inComboPool.Value.Has(targetEntity) == false)
                {
                    _inComboPool.Value.Add(targetEntity);
                }
                
                ref var inComboComp = ref _inComboPool.Value.Get(targetEntity);

                inComboComp.ComboConfig = targetProvideComboRequest.ComboConfig;
                if (inComboComp.ComboConfig == null || inComboComp.ComboConfig.AttackConfig == null)
                {
                    continue;
                }

                var attackSpeed = GetAttackSpeed(targetEntity);
                var attackDuration = inComboComp.ComboConfig.AttackConfig.GetEffectiveAnimationDuration(attackSpeed);
                
                if (inComboComp.ComboConfig.NextCombos.Count == 0)
                {
                    inComboComp.NextComboWindowStart = _cachedTime + attackDuration;
                    inComboComp.NextComboWindowEnd = _cachedTime + attackDuration;
                }
                else
                {
                    inComboComp.NextComboWindowStart = _cachedTime
                                                       + inComboComp.ComboConfig.AttackConfig.ComboWindow.x
                                                       * attackDuration;
                    inComboComp.NextComboWindowEnd = _cachedTime
                                                     + inComboComp.ComboConfig.AttackConfig.ComboWindow.y
                                                     * attackDuration;
                }
                
                ref var targetProvideAttackRequest = ref _targetProvideAttackRequestPool.Value.Add(_world.Value.NewEntity());
                targetProvideAttackRequest.AttackConfig = inComboComp.ComboConfig.AttackConfig;
                targetProvideAttackRequest.PackedEntity = targetProvideComboRequest.PackedEntity;
            }
        }

        private float GetAttackSpeed(int entity)
        {
            if (_unitStatsPool.Value.Has(entity) == false)
            {
                return 1f;
            }

            ref var unitStats = ref _unitStatsPool.Value.Get(entity);
            return unitStats.AttackSpeed > 0f ? unitStats.AttackSpeed : 1f;
        }
    }
}
