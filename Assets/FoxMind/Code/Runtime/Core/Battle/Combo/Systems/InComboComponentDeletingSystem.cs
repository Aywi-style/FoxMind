using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class InComboComponentDeletingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InComboComp>> _inComboFilter = default;

        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_inComboFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var inAttackEntity in _inComboFilter.Value)
            {
                ref var inComboComponent = ref _inComboPool.Value.Get(inAttackEntity);

                bool isWindowExpired = _cachedTime > inComboComponent.NextComboWindowEnd;
                bool hasNextCombos = inComboComponent.ComboConfig != null && inComboComponent.ComboConfig.NextCombos.Count > 0;
                bool isInAttackPhase = _inAttackPool.Value.Has(inAttackEntity) || _inAttackRecoveryPool.Value.Has(inAttackEntity);

                if (isWindowExpired || (hasNextCombos == false && isInAttackPhase == false))
                {
                    _inComboPool.Value.Del(inAttackEntity);
                }
            }
        }
    }
}
