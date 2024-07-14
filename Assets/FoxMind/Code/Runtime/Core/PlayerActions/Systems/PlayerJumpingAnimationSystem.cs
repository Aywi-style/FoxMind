using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerJumpingAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlayerControlledComp, AnimatorComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<AnimatorComp> _animatorPool = default;
        readonly EcsPoolInject<IsGroundedComp> _isGroundedPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var controlledEntity in _controlledTransformFilter.Value)
            {
                ref var animator = ref _animatorPool.Value.Get(controlledEntity);

                animator.Value.SetBool("IsGrounded", _isGroundedPool.Value.Has(controlledEntity));
            }
        }
    }
}