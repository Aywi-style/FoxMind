using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    public class RegisterTargetingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TargetingComp>, Exc<TargetingStateComp>> _targetingFilter = default;
        private readonly EcsPoolInject<TargetingStateComp> _targetingStatePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var state = ref _targetingStatePool.Value.Add(targetingEntity);
                state.IsPressed = false;
                state.PressStartTime = float.MinValue;
                state.LastTapTime = float.MinValue;
            }
        }
    }
}
