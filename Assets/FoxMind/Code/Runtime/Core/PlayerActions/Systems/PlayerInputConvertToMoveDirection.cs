using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Mathematics;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerInputConvertToMoveDirection : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlayerInputComp>> _inputEntityFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<PlayerInputComp> _playerInputPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        
        public void Run(IEcsSystems systems)
        {
            Vector3 cameraForward = new Vector3();
            Vector3 cameraRight = new Vector3();

            if (Camera.main != null)
            {
                cameraForward = Camera.main.transform.forward;
                cameraRight = Camera.main.transform.right;
            }
            
            foreach (var inputEntity in _inputEntityFilter.Value)
            {
                ref var input = ref _playerInputPool.Value.Get(inputEntity);
                
                foreach (var controlledEntity in _controlledTransformFilter.Value)
                {
                    ref var moveable = ref _moveablePool.Value.Get(controlledEntity);

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    
                    cameraForward = math.normalize(cameraForward);
                    cameraRight = math.normalize(cameraRight);
                    
                    moveable.NormalizedMoveDirection = (cameraForward * input.Direction.y) + (cameraRight * input.Direction.x);
                }
            }
        }
    }
}