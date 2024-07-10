using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Systems
{
    public class NewMoveAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MotionAnimationComp, MoveableComp, TransformComp, AnimancerComp>> _animableFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        readonly EcsPoolInject<MotionAnimationComp> _motionAnimationPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _animableFilter.Value)
            {
                ref var motionAnimation = ref _motionAnimationPool.Value.Get(movableEntity);

                if (motionAnimation.MoveState == null)
                {
                    continue;
                }
                
                if (motionAnimation.MoveState.IsActive == false)
                {
                    ref var animancer = ref _animancerPool.Value.Get(movableEntity);
                    if (animancer.Value.States.Current.NormalizedTime >= 1)
                    {
                        animancer.Value.TryPlay(motionAnimation.Move);
                    }
                }
                
                ref var transform = ref _transformPool.Value.Get(movableEntity);
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);

                float animationX = Vector3.Dot(transform.Value.right, moveable.NormalizedMoveDirection);
                float animationY = Vector3.Dot(transform.Value.forward, moveable.NormalizedMoveDirection);

                motionAnimation.MoveState.Parameter = new Vector2(animationX, animationY);
            }
        }
    }
}