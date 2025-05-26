using FoxMind.Code.Runtime.Core.Ecs.EcsAspects;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.AspectTest.Aspects
{
    public class AspectTest : IEcsAspect
    {
        public readonly EcsFilterInject<Inc<TransformComp>> FilterTransform;
        public readonly EcsPoolInject<TransformComp> TransformPool;
    }
}