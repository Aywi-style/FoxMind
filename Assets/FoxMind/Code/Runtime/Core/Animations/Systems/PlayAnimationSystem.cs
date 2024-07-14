using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Animations.Systems
{
    public class PlayAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world;
        
        private readonly EcsFilterInject<Inc<PlayAnimationRequest>> _playAnimationFilter = default;
        
        private readonly EcsPoolInject<PlayAnimationRequest> _playAnimationPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_playAnimationFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var playAnimationRequestEntity in _playAnimationFilter.Value)
            {
                ref var playAnimationRequest = ref _playAnimationPool.Value.Get(playAnimationRequestEntity);

                if (playAnimationRequest.Target.Unpack(_world.Value, out int unpackedEntity) == false)
                {
                    continue;
                }

                if (_animancerPool.Value.Has(unpackedEntity) == false)
                {
                    continue;
                }

                ref var animancerComp = ref _animancerPool.Value.Get(unpackedEntity);

                animancerComp.Value.Animator.applyRootMotion = playAnimationRequest.IsRootMotion;
                var state = animancerComp.Value.Play(playAnimationRequest.Clip);
                state.Time = 0;
            }
        }
    }
}