using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система отработки запроса на удаление ImmovableBecauseInAttack компонента
    /// </summary>
    public class DeletingImmovableBecauseInAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SelfUnImmovableBecauseInAttackRequest, ImmovableBecauseInAttackComp>> _immovableFilter = default;

        private readonly EcsPoolInject<ImmovableBecauseInAttackComp> _immovablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_immovableFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var nonImmovableEntity in _immovableFilter.Value)
            {
                _immovablePool.Value.Del(nonImmovableEntity);
            }
        }
    }
}