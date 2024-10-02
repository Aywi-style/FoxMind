using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.PlayerActions.Systems;

namespace FoxMind.Code.Runtime.Core.PlayerActions.SystemsAssembly
{
    public class PlayerActionAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new PlayerDashSystem(),
                new PlayerRotateSystem(),
                new PlayerInputConvertToMoveDirection(),
                new PlayerIsGroundedSystem(),
                new PlayerJumpingAnimationSystem()
            };
        }
    }
}