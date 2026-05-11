using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Передаёт данные InHitReactionComp в HitReactionMovementBehaviour и временно переключает контроллер цели на него.
    /// </summary>
    public class ApplyHitReactionMovementSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InHitReactionComp, MoveableComp, MoveableBehavioursComp>> _hitReactionMovementFilter = default;

        private readonly EcsPoolInject<InHitReactionComp> _inHitReactionPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<MoveableBehavioursComp> _moveableBehavioursPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _hitReactionMovementFilter.Value)
            {
                ref var inHitReaction = ref _inHitReactionPool.Value.Get(entity);
                ref var moveable = ref _moveablePool.Value.Get(entity);
                ref var moveableBehaviours = ref _moveableBehavioursPool.Value.Get(entity);

                if (moveable.CustomCharacterController == null || moveableBehaviours.MovementBehaviours == null)
                {
                    continue;
                }

                if (moveableBehaviours.MovementBehaviours.TryGetValue(BehavioursConstants.HitReaction, out var movementBehaviour) == false)
                {
                    movementBehaviour = new HitReactionMovementBehaviour();
                    movementBehaviour.Initialize(moveable.Motor);
                    moveableBehaviours.MovementBehaviours.Add(BehavioursConstants.HitReaction, movementBehaviour);
                }

                if (movementBehaviour is not HitReactionMovementBehaviour hitReactionMovementBehaviour)
                {
                    continue;
                }

                hitReactionMovementBehaviour.Configure(
                    inHitReaction.Type,
                    inHitReaction.WorldReactionVelocity,
                    inHitReaction.WaitForGroundBeforeTimer);

                if (moveable.CustomCharacterController.CurrentMovementBehaviour != hitReactionMovementBehaviour)
                {
                    moveable.CustomCharacterController.SetCurrentMovementBehaviour(hitReactionMovementBehaviour);
                }
            }
        }
    }
}
