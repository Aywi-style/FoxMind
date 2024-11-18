using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Mathematics;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerInputConvertToMoveDirection : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        readonly EcsFilterInject<Inc<CameraComp, TransformComp>> _cameraFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        
        public void Run(IEcsSystems systems)
        {
            Vector3 cameraForward = new Vector3();
            Vector3 cameraRight = new Vector3();

            if (_cameraFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var cameraEntity in _cameraFilter.Value)
                {
                    ref var cameraTransform = ref _transformPool.Value.Get(cameraEntity);
                    cameraForward = cameraTransform.Value.forward;
                    cameraRight = cameraTransform.Value.right;
                    
                    break;
                }
            }
            
            foreach (var inputEntity in _inputDirectionFilter.Value)
            {
                ref var input = ref _inputDirectionPool.Value.Get(inputEntity);
                
                foreach (var controlledEntity in _controlledTransformFilter.Value)
                {
                    ref var moveable = ref _moveablePool.Value.Get(controlledEntity);

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    
                    cameraForward = Vector3.Normalize(cameraForward);
                    cameraRight = Vector3.Normalize(cameraRight);
                    
                    moveable.NormalizedMoveDirection = (cameraForward * input.Direction.y) + (cameraRight * input.Direction.x);
                }
            }
        }
    }
}