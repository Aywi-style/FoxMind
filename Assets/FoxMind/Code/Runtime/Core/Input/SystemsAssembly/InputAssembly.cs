using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Input.Systems;
using FoxMind.Code.Runtime.Core.SystemsAssembly;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Input.SystemsAssembly
{
    public class InputAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new PlayerInputSystem()
            };
        }
    }
}