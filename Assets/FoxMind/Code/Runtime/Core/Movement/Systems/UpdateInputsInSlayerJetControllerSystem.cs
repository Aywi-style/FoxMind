using System;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система, передающая NormalizedMoveDirection в контроллер главного героя
    /// </summary>
    public class UpdateInputsInSlayerJetControllerSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SlayerJetCharacterControllerComp, MoveableComp, MoveableBehavioursComp>> _slayerJetControllerFilter = default;

        private readonly EcsPoolInject<SlayerJetCharacterControllerComp> _slayerJetCharacterControllerPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;
        private readonly EcsPoolInject<ImmovableBecauseInAttackComp> _immovableBecauseInAttackPool = default;
        private readonly EcsPoolInject<MoveableBehavioursComp> _moveableBehavioursPool = default;

        private Vector3 _cachedMoveVelocity;
        private IMovementBehaviour _cachedNewController;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _slayerJetControllerFilter.Value)
            {
                _cachedNewController = null;
                
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);
                ref var moveableBehaviours = ref _moveableBehavioursPool.Value.Get(movableEntity);
                
                var immovableMultiply = _immovablePool.Value.Has(movableEntity) ? 0 : 1;

                float immovableBecauseInAttackMultiply = 1;
                switch (moveable.CustomCharacterController.Motor.GroundingStatus.IsStableOnGround)
                {
                    case true when _immovableBecauseInAttackPool.Value.Has(movableEntity):
                        immovableBecauseInAttackMultiply = 0;
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionStable, out _cachedNewController);
                        
                        break;
                    case true when _immovableBecauseInAttackPool.Value.Has(movableEntity) == false:
                        immovableBecauseInAttackMultiply = 1;
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.Stable, out _cachedNewController);
                        
                        break;
                    case false when _immovableBecauseInAttackPool.Value.Has(movableEntity):
                        immovableBecauseInAttackMultiply = 0;
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionAir, out _cachedNewController);
                        
                        break;
                    case false when _immovableBecauseInAttackPool.Value.Has(movableEntity) == false:
                        immovableBecauseInAttackMultiply = 1;
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.Air, out _cachedNewController);
                        
                        break;
                    default:
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.Stable, out _cachedNewController);
                        break;
                }

                if (_cachedNewController != null && _cachedNewController != moveable.CustomCharacterController.CurrentMovementBehaviour)
                {
                    moveable.CustomCharacterController.SetCurrentMovementBehaviour(_cachedNewController);
                }
                
                moveable.CustomCharacterController.SetMoveDirection(moveable.NormalizedMoveDirection * immovableMultiply * immovableBecauseInAttackMultiply);
            }
        }
    }
}