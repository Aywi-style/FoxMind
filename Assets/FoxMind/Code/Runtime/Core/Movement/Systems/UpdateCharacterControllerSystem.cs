using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Общая система выбора movement behaviour и передачи направления в CustomCharacterController.
    /// Работает и для игрока и для врагов; источник направления задаётся отдельными input/AI системами через MoveableComp.
    /// </summary>
    public class UpdateCharacterControllerSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterControllerComp, MoveableComp, MoveableBehavioursComp>> _characterControllerFilter = default;

        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;
        private readonly EcsPoolInject<InHitReactionComp> _inHitReactionPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<MoveableBehavioursComp> _moveableBehavioursPool = default;

        private IMovementBehaviour _cachedNewController;

        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _characterControllerFilter.Value)
            {
                _cachedNewController = null;

                ref var moveable = ref _moveablePool.Value.Get(movableEntity);
                ref var moveableBehaviours = ref _moveableBehavioursPool.Value.Get(movableEntity);
                if (moveable.CustomCharacterController == null || moveable.CustomCharacterController.Motor == null || moveableBehaviours.MovementBehaviours == null)
                {
                    continue;
                }

                var immovableMultiply = _immovablePool.Value.Has(movableEntity) ? 0 : 1;
                var movementInputMultiply = SelectMovementBehaviour(movableEntity, ref moveable, ref moveableBehaviours);

                if (_cachedNewController != null && _cachedNewController != moveable.CustomCharacterController.CurrentMovementBehaviour)
                {
                    moveable.CustomCharacterController.SetCurrentMovementBehaviour(_cachedNewController);
                }

                moveable.CustomCharacterController.SetMoveDirection(moveable.NormalizedMoveDirection * immovableMultiply * movementInputMultiply);
                moveable.CustomCharacterController.SetLookDirection(moveable.NormalizedLookDirection);
            }
        }

        private float SelectMovementBehaviour(int movableEntity, ref MoveableComp moveable, ref MoveableBehavioursComp moveableBehaviours)
        {
            if (_inHitReactionPool.Value.Has(movableEntity))
            {
                moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.HitReaction, out _cachedNewController);
                return 0f;
            }

            if (_inAttackPool.Value.Has(movableEntity))
            {
                ref var attackComp = ref _inAttackPool.Value.Get(movableEntity);
                
                switch (attackComp.AttackConfig.RequiredMovementMode)
                {
                    case AttackMovementMode.GroundRootMotion:
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionStable, out _cachedNewController);
                        return 0f;
                    case AttackMovementMode.AirRootMotion:
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionAir, out _cachedNewController);
                        return 0f;
                    default:
                        return 1f;
                }
            }

            var isGrounded = moveable.CustomCharacterController.Motor.GroundingStatus.IsStableOnGround;

            moveableBehaviours.MovementBehaviours.TryGetValue(isGrounded ? BehavioursConstants.Stable : BehavioursConstants.Air, out _cachedNewController);
            
            return 1f;
        }
    }
}
