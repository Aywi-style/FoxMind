using System;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Object = UnityEngine.Object;

namespace FoxMind.Code.Runtime.Core.Scenes.Init.Systems
{
    [Serializable]
    public class SceneInitSystem : BaseEcsVisitable, IEcsPreInitSystem
    {
        public void PreInit(IEcsSystems systems)
        {
            var sceneObjects = Object.FindObjectsOfType<EntityBaker>();
            
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.Init(systems.GetWorld());
            }
        }
    }
}