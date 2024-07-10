using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Scenes.Init.Systems;
using FoxMind.Code.Runtime.Core.SystemsAssembly;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;

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