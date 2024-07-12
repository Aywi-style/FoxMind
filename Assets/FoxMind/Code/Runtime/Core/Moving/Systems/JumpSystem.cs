using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Systems
{
    public class JumpSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputJumpEvent>> _inputJumpFilter = default;
        private readonly EcsFilterInject<Inc<RigidBodyComp, MoveableComp, JumpableComp, AnimatorComp>> _movableFilter = default;

        private readonly EcsPoolInject<RigidBodyComp> _rigidBodyPool = default;
        private readonly EcsPoolInject<AnimatorComp> _animatorPool = default;
        private readonly EcsPoolInject<JumpableComp> _jumpablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_inputJumpFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var movableEntity in _movableFilter.Value)
            {
                ref var rigidBody = ref _rigidBodyPool.Value.Get(movableEntity);
                ref var jumpable = ref _jumpablePool.Value.Get(movableEntity);
                ref var animator = ref _animatorPool.Value.Get(movableEntity);
            
                _cachedMoveVelocity = rigidBody.Value.velocity;
                _cachedMoveVelocity.y = 0;
                rigidBody.Value.velocity = _cachedMoveVelocity;
                rigidBody.Value.AddForce(Vector3.up * jumpable.JumpImpulse, ForceMode.VelocityChange);

                animator.Value.SetTrigger("Jump");
                
                jumpable.IsJumping = false;
                jumpable.IsGrounded = false;
            }
        }
    }
}