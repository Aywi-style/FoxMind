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
            ref var tComponent = ref world.GetPool<T>().Add(entity);
            SetComposeValues(ref tComponent);
        }

        public void SetComposeValues(ref T component)
        {
            component = (T)this;
        }
    }
}