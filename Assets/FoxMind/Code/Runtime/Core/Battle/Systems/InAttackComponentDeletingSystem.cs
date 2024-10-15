using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    public class InAttackComponentDeletingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;

        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<SelfUnImmovableRequest> _selfUnImmovableRequestPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_inAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var inAttackEntity in _inAttackFilter.Value)
            {
                ref var inAttackComponent = ref _inAttackPool.Value.Get(inAttackEntity);

                if (_cachedTime > inAttackComponent.End)
                {
                    /*foreach (var attackComponent in inAttackComponent.AttackConfig.AttackEndComponents)
                    {
                        attackComponent.Compose(_world.Value, inAttackEntity);
                    }*/

                    _inAttackPool.Value.Del(inAttackEntity);

                    if (_selfUnImmovableRequestPool.Value.Has(inAttackEntity) == false)
                    {
                        _selfUnImmovableRequestPool.Value.Add(inAttackEntity);
                    }
                }
            }
        }
    }
}