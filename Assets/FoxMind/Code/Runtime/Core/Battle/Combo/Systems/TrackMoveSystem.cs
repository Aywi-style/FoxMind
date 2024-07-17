using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class TrackMoveSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private const float c_deadZone = 0.1f;
        
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp, InputDirectionComp>> _inputDashFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, CombinableComp, TransformComp, MoveableComp, InputtedForwardMoveComp, InputtedBackwardMoveComp, InputtedLeftMoveComp, InputtedRightMoveComp>> _comboFilter = default;

        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;
        
        private readonly EcsPoolInject<MoveableComp> _mobablePool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var comboEntity in _comboFilter.Value)
            {
                float time = Time.time;
                
                ref var inputtedForward = ref _inputtedForwardMovePool.Value.Get(comboEntity);
                ref var inputtedBackward = ref _inputtedBackwardMovePool.Value.Get(comboEntity);
                ref var inputtedLeft = ref _inputtedLeftMovePool.Value.Get(comboEntity);
                ref var inputtedRight = ref _inputtedRightMovePool.Value.Get(comboEntity);
                
                ref var moveable = ref _mobablePool.Value.Get(comboEntity);
                ref var transform = ref _transformPool.Value.Get(comboEntity);
                
                float dotX = Vector3.Dot(transform.Value.right, moveable.NormalizedMoveDirection);
                float dotY = Vector3.Dot(transform.Value.forward, moveable.NormalizedMoveDirection);

                switch (dotX)
                {
                    case > c_deadZone:
                        inputtedRight.LastPress = time;
                        break;
                    case < -c_deadZone:
                        inputtedLeft.LastPress = time;
                        break;
                }

                switch (dotY)
                {
                    case > c_deadZone:
                        inputtedForward.LastPress = time;
                        break;
                    case < -c_deadZone:
                        inputtedBackward.LastPress = time;
                        break;
                }
            }
        }
    }
}