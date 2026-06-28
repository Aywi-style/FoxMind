using Animancer;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Инициализирует IMovementBehaviour и IJumpBehaviour внутри MoveableComp
    /// </summary>
    public class RegisterMoveableRequestSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MoveableComp, MoveableBehavioursComp>, Exc<RegisteredMoveableBehavioursTag>> _requestFilter = default;

        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<MoveableBehavioursComp> _moveableBehavioursPool = default;
        private readonly EcsPoolInject<RegisteredMoveableBehavioursTag> _registeredMoveableBehavioursPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _requestFilter.Value)
            {
                ref var moveableComp = ref _moveablePool.Value.Get(movableEntity);
                ref var moveableBehaviours = ref _moveableBehavioursPool.Value.Get(movableEntity);

                foreach (var movementBehaviour in moveableBehaviours.MovementBehaviours.Values)
                {
                    movementBehaviour.Initialize(moveableComp.Motor);
                }
                
                moveableComp.CustomCharacterController.SetJumpBehaviour(moveableBehaviours.JumpBehaviour);
                moveableComp.CustomCharacterController.SetDashBehaviour(moveableBehaviours.DashBehaviour);
                
                moveableBehaviours.JumpBehaviour?.Initialize(moveableComp.Motor);
                moveableBehaviours.DashBehaviour?.Initialize(moveableComp.Motor);

                _registeredMoveableBehavioursPool.Value.Add(movableEntity);
            }
        }
    }
}
