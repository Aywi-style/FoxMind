using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Camera.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Camera.SystemsAssembly
{
    public class CameraAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new CameraTargetMoveSystem()
            };
        }
    }
}