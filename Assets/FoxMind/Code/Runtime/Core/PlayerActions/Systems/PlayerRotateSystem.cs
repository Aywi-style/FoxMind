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
    public class PlayerRotateSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        
        public void Run(IEcsSystems systems)
        {
            Vector3 cameraForward = new Vector3();
            Vector3 cameraRight = new Vector3();

            if (Camera.main != null)
            {
                cameraForward = Camera.main.transform.forward;
                cameraRight = Camera.main.transform.right;
            }
            
            foreach (var inputEntity in _inputDirectionFilter.Value)
            {
                ref var input = ref _inputDirectionPool.Value.Get(inputEntity);
                
                foreach (var controlledEntity in _controlledTransformFilter.Value)
                {
                    ref var transform = ref _transformPool.Value.Get(controlledEntity);

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    
                    cameraForward = math.normalize(cameraForward);
                    cameraRight = math.normalize(cameraRight);
                    
                    var moveDirection = (cameraForward * input.Direction.y) + (cameraRight * input.Direction.x);

                    if (math.lengthsq(input.Direction) > float.Epsilon)
                    {
                        var forward = new float3(moveDirection.x, 0f, moveDirection.z);
                        Quaternion lookRotation = quaternion.LookRotation(forward, math.up());
                        transform.Value.rotation = Quaternion.Slerp(transform.Value.rotation, lookRotation, 0.2f);
                    }
                }
            }
        }
    }
}