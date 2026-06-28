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
    public class SetMoveAndLookDirectionSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterControllerComp, MoveableComp, MoveableBehavioursComp>> _characterControllerFilter = default;

        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;
        private readonly EcsPoolInject<InHitReactionComp> _inHitReactionPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _characterControllerFilter.Value)
            {
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);

                moveable.CustomCharacterController.SetMoveDirection(moveable.NormalizedMoveDirection * GetMovableMultiply(movableEntity));
                moveable.CustomCharacterController.SetLookDirection(moveable.NormalizedLookDirection);
            }
        }

        private float GetMovableMultiply(int movableEntity)
        {
            if (_inAttackPool.Value.Has(movableEntity) || _inHitReactionPool.Value.Has(movableEntity) || _immovablePool.Value.Has(movableEntity))
            {
                return 0f;
            }

            return 1f;
        }
    }
}