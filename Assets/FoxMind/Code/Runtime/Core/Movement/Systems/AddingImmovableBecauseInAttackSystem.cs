using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система отработки запроса на добавление ImmovableBecauseInAttack компонента
    /// </summary>
    public class AddingImmovableBecauseInAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SelfImmovableBecauseInAttackRequest>, Exc<ImmovableBecauseInAttackComp>> _nonImmovableBecauseInAttackFilter = default;

        private readonly EcsPoolInject<ImmovableBecauseInAttackComp> _immovableBecauseInAttackPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_nonImmovableBecauseInAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var nonImmovableEntity in _nonImmovableBecauseInAttackFilter.Value)
            {
                _immovableBecauseInAttackPool.Value.Add(nonImmovableEntity);
            }
        }
    }
}