using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputAttackEvent>> _inputAttackEventFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, AnimancerComp>> _controlledFilter = default;
        
        readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_inputAttackEventFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var controlledEntity in _controlledFilter.Value)
            {
                ref var animancer = ref _animancerPool.Value.Get(controlledEntity);

                animancer.Value.Animator.applyRootMotion = true;
                /*var state = animancer.Value.Play(animancer.Clip);
                state.Time = 0;*/
            }
        }
    }
}