using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.Extensions;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Animations.Systems
{
    public class SelfPlayAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world;
        
        private readonly EcsFilterInject<Inc<SelfPlayAnimationRequest>> _selfPlayAnimationFilter = default;
        
        private readonly EcsPoolInject<SelfPlayAnimationRequest> _selfPlayAnimationPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_selfPlayAnimationFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var selfPlayAnimationRequestEntity in _selfPlayAnimationFilter.Value)
            {
                ref var playAnimationRequest = ref _selfPlayAnimationPool.Value.Get(selfPlayAnimationRequestEntity);
                
                if (_animancerPool.Value.Has(selfPlayAnimationRequestEntity) == false)
                {
                    continue;
                }

                ref var animancerComp = ref _animancerPool.Value.Get(selfPlayAnimationRequestEntity);

                animancerComp.Value.Animator.applyRootMotion = playAnimationRequest.IsRootMotion;
                var state = animancerComp.Value.Play(playAnimationRequest.Clip);
                state.Time = 0;
            }
        }
    }
}