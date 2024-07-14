using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    public class JumpSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SelfJumpRequest, RigidBodyComp, JumpableComp>> _jumpFilter = default;

        private readonly EcsPoolInject<RigidBodyComp> _rigidBodyPool = default;
        private readonly EcsPoolInject<JumpableComp> _jumpablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_jumpFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var movableEntity in _jumpFilter.Value)
            {
                ref var rigidBody = ref _rigidBodyPool.Value.Get(movableEntity);
                ref var jumpable = ref _jumpablePool.Value.Get(movableEntity);
            
                _cachedMoveVelocity = rigidBody.Value.velocity;
                _cachedMoveVelocity.y = 0;
                rigidBody.Value.velocity = _cachedMoveVelocity;
                rigidBody.Value.AddForce(Vector3.up * jumpable.Impulse, ForceMode.VelocityChange);
            }
        }
    }
}