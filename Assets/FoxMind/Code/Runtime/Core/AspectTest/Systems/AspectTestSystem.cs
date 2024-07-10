using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.AspectTest.Systems
{
    public class AspectTestSystem : IEcsInitSystem
    {
        private EcsCustomInject<Aspects.AspectTest> _aspectTest;
        
        public void Init(IEcsSystems systems)
        {
            Debug.Log(_aspectTest.Value.world.Value);
        }
    }
}