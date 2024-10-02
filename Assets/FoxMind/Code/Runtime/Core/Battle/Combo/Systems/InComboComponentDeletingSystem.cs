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

                if (_cachedTime > inComboComponent.NextComboWindowEnd)
                {
                    _inComboPool.Value.Del(inAttackEntity);
                }
            }
        }
    }
}