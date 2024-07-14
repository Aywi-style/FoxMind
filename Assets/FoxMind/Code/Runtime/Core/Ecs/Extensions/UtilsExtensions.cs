using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Ecs.Extensions
{
    public static class UtilsExtensions
    {
        public static bool TryGet<T>(this EcsPoolInject<T> poolInject, int entity, out T comp) where T : struct
        {
            if (poolInject.Value.Has(entity))
            {
                comp = poolInject.Value.Get(entity);

                return true;
            }

            comp = default;
            return false;
        }
    }
}