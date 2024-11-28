using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Система, которая удаляет компонент InAttackComp в момент конца окна для нанесения урона
    /// </summary>
    public class TransitionFromAttackToAttackOveringSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;

        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;

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

                if (_cachedTime < inAttackComponent.End)
                {
                    continue;
                }
                /*foreach (var attackComponent in inAttackComponent.AttackConfig.AttackEndComponents)
                    {
                        attackComponent.Compose(_world.Value, inAttackEntity);
                    }*/

                _inAttackPool.Value.Del(inAttackEntity);
            }
        }
    }
}