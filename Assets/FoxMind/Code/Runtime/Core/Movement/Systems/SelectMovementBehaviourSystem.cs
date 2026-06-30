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
    public class SelectMovementBehaviourSystem : BaseEcsVisitable, IEcsRunSystem
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

                SelectMovementBehaviour(movableEntity, ref moveable, ref moveableBehaviours);

                if (_cachedNewController == null || _cachedNewController == moveable.CustomCharacterController.CurrentMovementBehaviour)
                {
                    continue;
                }
                
                moveable.CustomCharacterController.SetCurrentMovementBehaviour(_cachedNewController);
            }
        }

        private void SelectMovementBehaviour(int movableEntity, ref MoveableComp moveable, ref MoveableBehavioursComp moveableBehaviours)
        {
            if (_inHitReactionPool.Value.Has(movableEntity))
            {
                moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.HitReaction, out _cachedNewController);
                return;
            }

            if (_inAttackPool.Value.Has(movableEntity))
            {
                ref var attackComp = ref _inAttackPool.Value.Get(movableEntity);
                
                switch (attackComp.AttackConfig.RequiredMovementMode)
                {
                    case AttackMovementMode.GroundRootMotion:
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionStable, out _cachedNewController);
                        return;
                    case AttackMovementMode.AirRootMotion:
                        moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.RootMotionAir, out _cachedNewController);
                        return;
                    default:
                        _cachedNewController = UnknownMovementBehaviour.Get();
                        return;
                }
            }

            var isGrounded = moveable.CustomCharacterController.Motor.GroundingStatus.IsStableOnGround;

            moveableBehaviours.MovementBehaviours.TryGetValue(isGrounded ? BehavioursConstants.Stable : BehavioursConstants.Air, out _cachedNewController);
        }
    }
}