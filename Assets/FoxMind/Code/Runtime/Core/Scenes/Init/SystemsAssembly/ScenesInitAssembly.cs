using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.Scenes.Init.Systems;

namespace FoxMind.Code.Runtime.Core.Scenes.Init.SystemsAssembly
{
    public class ScenesInitAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new SceneInitSystem()
            };
        }
    }
}