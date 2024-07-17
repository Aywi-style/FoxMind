using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Ecs.Templates
{
    public interface IEntityFeature
    {
        void Compose(EcsWorld world, int entity);
    }
    
    public interface IEntityFeature<T> : IEntityFeature where T : struct
    {
        void IEntityFeature.Compose(EcsWorld world, int entity)
        {
            var pool = world.GetPool<T>();
            if (pool.Has(entity))
            {
                ref var tComp = ref pool.Get(entity);
                SetComposeValues(ref tComp);
            }
            else
            {
                ref var tComp = ref pool.Add(entity);
                SetComposeValues(ref tComp);
            }
        }

        public void SetComposeValues(ref T component)
        {
            component = (T)this;
        }
    }
}