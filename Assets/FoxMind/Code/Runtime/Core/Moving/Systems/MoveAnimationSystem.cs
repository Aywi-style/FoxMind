using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Systems
{
    public class MoveAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<RigidBodyComp, MoveableComp, AnimatorComp>> _animableFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        readonly EcsPoolInject<AnimatorComp> _animatorPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _animableFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(movableEntity);
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);
                ref var animator = ref _animatorPool.Value.Get(movableEntity);

                float animationX = Vector3.Dot(transform.Value.right, moveable.NormalizedMoveDirection);
                float animationY = Vector3.Dot(transform.Value.forward, moveable.NormalizedMoveDirection);

                animator.Value.SetFloat("MoveX", animationX);
                animator.Value.SetFloat("MoveY", animationY);
            }
        }
    }
}