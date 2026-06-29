using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    public class UpdateDirectionToTargetSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<TransformComp, TargetingComp>> _targetingFilter = default;
        
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                ref var transform = ref _transformPool.Value.Get(targetingEntity);

                if (targeting.HasHardTarget == false)
                {
                    continue;
                }

                targeting.HardTarget.Unpack(_world.Value, out var targetEntity);

                if (_targetFindablePool.Value.Has(targetEntity) == false)
                {
                    continue;
                }
                
                ref var targetFindable = ref _targetFindablePool.Value.Get(targetEntity);
                
                targeting.HardTargetForwardDirection = (targetFindable.TargetPoint.position - transform.Value.position).normalized;
            }
        }
    }
}