using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    public class AddingImmovableCompSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SelfImmovableRequest>, Exc<ImmovableComp>> _nonImmovableFilter = default;

        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_nonImmovableFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var nonImmovableEntity in _nonImmovableFilter.Value)
            {
                _immovablePool.Value.Add(nonImmovableEntity);
            }
        }
    }
}