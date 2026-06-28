using Animancer;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система устанавливающая локомотив анимации для сущности
    /// </summary>
    public class RegisterMotionAnimationsSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MotionAnimationComp, AnimancerComp>, Exc<InAttackComp, InDefenceComp, InHitReactionComp>> _requestFilter = default;

        private readonly EcsPoolInject<MotionAnimationComp> _motionAnimationPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _requestFilter.Value)
            {
                ref var motionAnimation = ref _motionAnimationPool.Value.Get(movableEntity);

                if (motionAnimation.MoveState != null && motionAnimation.MoveState.IsActive)
                {
                    continue;
                }
                
                ref var animancer = ref _animancerPool.Value.Get(movableEntity);
                animancer.Value.Animator.applyRootMotion = false;
                
                var state = animancer.Value.States.GetOrCreate(motionAnimation.MoveAsset);
                motionAnimation.MoveState = (MixerState<Vector2>)state;
                //animancer.Value.Play(motionAnimation.MoveState, 0.2f);
                animancer.Value.Play(motionAnimation.MoveState);
            }
        }
    }
}
