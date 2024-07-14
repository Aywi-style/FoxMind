using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Ecs.RequestsDeleting.Abstract
{
    public abstract class BaseRequestDeleteSystem<T> : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem where T : struct
    {
        private EcsFilter _filter;
        private EcsPool<T> _pool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<T>().End();
            _pool = world.GetPool<T>();
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_filter.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var entity in _filter)
            {
                _pool.Del(entity);
            }
        }
    }
}