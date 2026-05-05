using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Уничтожает сущность и её визуализацию, если на ней был компонент VisualHolderComp
    /// </summary>
    public class DeathSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<DeathRequest>> _deathRequestFilter = default;

        private readonly EcsPoolInject<DeathRequest> _deathRequestPool = default;
        private readonly EcsPoolInject<VisualHolderComp> _visualHolderPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var deathRequestEntity in _deathRequestFilter.Value)
            {
                ref var deathRequestComp = ref _deathRequestPool.Value.Get(deathRequestEntity);

                if (deathRequestComp.For.Unpack(_world.Value, out var entity) == false)
                {
                    continue;
                }

                if (_visualHolderPool.Value.Has(entity))
                {
                    ref var visualHolderComponent = ref _visualHolderPool.Value.Get(entity);
                    Object.Destroy(visualHolderComponent.Value);
                }
                
                _world.Value.DelEntity(entity);
            }
        }
    }
}