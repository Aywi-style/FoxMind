using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.PlayerActions.Systems;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.PlayerActions.SystemsAssembly
{
    public class PlayerActionAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new PlayerAttackTestSystem(),
                new PlayerDashSystem(),
                new PlayerRotateSystem(),
                new PlayerInputConvertToMoveDirection(),
                new PlayerIsGroundedSystem(),
                new PlayerJumpingAnimationSystem()
            };
        }
    }
}