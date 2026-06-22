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
    public class PlayerDefenceSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;
        
        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        
        private Vector3 _cachedDashPosition;

        public void Run(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);
                if (inputControlsComp.Value.GeneralMap.Defence.WasPressedThisFrame() == false)
                {
                    return;
                }
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