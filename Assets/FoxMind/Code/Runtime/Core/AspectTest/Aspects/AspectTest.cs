using FoxMind.Code.Runtime.Core.Ecs.Aspects;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.AspectTest.Aspects
{
    public class AspectTest : IEcsAspect
    {
        public EcsWorldInject world;
        public EcsFilterInject<Inc<TransformComp>> filterTransform;
    }
}