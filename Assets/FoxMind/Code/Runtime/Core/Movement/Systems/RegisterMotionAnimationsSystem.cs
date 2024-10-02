using Animancer;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    public class RegisterMotionAnimationsSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RegisterMotionAnimationRequest, MotionAnimationComp, AnimancerComp>> _requestFilter = default;

        private readonly EcsPoolInject<MotionAnimationComp> _motionAnimationPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _requestFilter.Value)
            {
                ref var motionAnimation = ref _motionAnimationPool.Value.Get(movableEntity);

                ref var animancer = ref _animancerPool.Value.Get(movableEntity);
                
                var state = animancer.Value.States.GetOrCreate(motionAnimation.Move);
                motionAnimation.MoveState = (MixerState<Vector2>)state;
                animancer.Value.Play(motionAnimation.MoveState, 0.2f);
            }
        }
    }
}