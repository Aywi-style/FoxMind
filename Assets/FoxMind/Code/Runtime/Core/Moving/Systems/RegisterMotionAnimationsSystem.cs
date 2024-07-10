using Animancer;
using FoxMind.Code.Runtime.Core.Moving.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Systems
{
    public class RegisterMotionAnimationsSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MotionAnimationComp, AnimancerComp>> _movableFilter = default;

        private readonly EcsPoolInject<MotionAnimationComp> _motionAnimationPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _movableFilter.Value)
            {
                ref var motionAnimation = ref _motionAnimationPool.Value.Get(movableEntity);

                if (motionAnimation.MoveState == null)
                {
                    ref var animancer = ref _animancerPool.Value.Get(movableEntity);
                    
                    //motionAnimation.MoveState = (MixerState<Vector2>)animancer.Value.States.GetOrCreate(motionAnimation.Move);
                    var state = animancer.Value.States.GetOrCreate(motionAnimation.Move);
                    motionAnimation.MoveState = (MixerState<Vector2>)state;
                    animancer.Value.Play(motionAnimation.Move);
                }
            }
        }
    }
}