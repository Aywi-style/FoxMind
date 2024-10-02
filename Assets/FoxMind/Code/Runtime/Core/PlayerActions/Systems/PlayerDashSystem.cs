using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerDashSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputDashEvent>> _inputDashFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        
        private Vector3 _cachedDashPosition;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var transformEntity in _controlledTransformFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(transformEntity);
                ref var moveable = ref _moveablePool.Value.Get(transformEntity);

                _cachedDashPosition.x = moveable.NormalizedMoveDirection.x * 5;
                _cachedDashPosition.y = 0;
                _cachedDashPosition.z = moveable.NormalizedMoveDirection.z * 5;
                
                transform.Value.position += _cachedDashPosition;
            }
        }
    }
}