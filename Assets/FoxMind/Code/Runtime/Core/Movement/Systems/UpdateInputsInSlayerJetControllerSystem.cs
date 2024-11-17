using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
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
        private readonly EcsFilterInject<Inc<SlayerJetCharacterControllerComp, MoveableComp>> _slayerJetControllerFilter = default;

        private readonly EcsPoolInject<SlayerJetCharacterControllerComp> _slayerJetCharacterControllerPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<ImmovableComp> _immovablePool = default;
        private readonly EcsPoolInject<ImmovableBecauseInAttackComp> _immovableBecauseInAttackPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _slayerJetControllerFilter.Value)
            {
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);
                ref var slayerJetCharacterController = ref _slayerJetCharacterControllerPool.Value.Get(movableEntity);
                
                var immovableMultiply = _immovablePool.Value.Has(movableEntity) ? 0 : 1;

                var immovableBecauseInAttackMultiply = 1;
                slayerJetCharacterController.Value.RootMotion = false;
                if (_immovableBecauseInAttackPool.Value.Has(movableEntity))
                {
                    immovableBecauseInAttackMultiply = 0;
                    slayerJetCharacterController.Value.RootMotion = true;
                }
                
                slayerJetCharacterController.Value.SetMoveDirection(moveable.NormalizedMoveDirection * immovableMultiply * immovableBecauseInAttackMultiply);
            }
        }
    }
}