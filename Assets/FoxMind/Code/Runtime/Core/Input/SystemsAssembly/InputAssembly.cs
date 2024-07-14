using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.Input.Systems;

namespace FoxMind.Code.Runtime.Core.Input.SystemsAssembly
{
    public class InputAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new InitBaseInputControlsSystem(),
                new InputDirectionSystem(),
                new InputAttackSystem(),
                new InputJumpSystem(),
                new InputDashSystem(),
            };
        }
    }
}