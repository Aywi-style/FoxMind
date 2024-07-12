using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Systems
{
    public class MoveSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RigidBodyComp, MoveableComp>> _movableFilter = default;

        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<RigidBodyComp> _rigidBodyPool = default;
        private readonly EcsPoolInject<AnimatorComp> _animatorPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _movableFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(movableEntity);
                ref var rigidBody = ref _rigidBodyPool.Value.Get(movableEntity);
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);

                _cachedMoveVelocity = rigidBody.Value.velocity;
                _cachedMoveVelocity.x = moveable.NormalizedMoveDirection.x * moveable.Speed;
                _cachedMoveVelocity.z = moveable.NormalizedMoveDirection.z * moveable.Speed;
                
                rigidBody.Value.velocity = _cachedMoveVelocity;
                //transform.Value.position += moveable.MoveDirection * moveable.Speed * deltaTime;
            }
        }
    }
}