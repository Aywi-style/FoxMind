using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    public class DeletingImmovableCompSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SelfUnImmovableRequest, ImmovableComp>> _immovableFilter = default;

        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;

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