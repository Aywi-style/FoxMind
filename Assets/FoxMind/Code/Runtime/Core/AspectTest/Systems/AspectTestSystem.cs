using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.AspectTest.Systems
{
    public class AspectTestSystem : BaseEcsVisitable, IEcsInitSystem
    {
        private EcsCustomInject<Aspects.AspectTest> _aspectTest;
        
        public void Init(IEcsSystems systems)
        {
            int i = 0;
            foreach (var transformEntity in _aspectTest.Value.FilterTransform.Value)
            {
                ref var gg = ref _aspectTest.Value.TransformPool.Value.Get(transformEntity);
                
                Debug.Log($"{i++}: {gg.Value.position}");
            }
        }
    }
}