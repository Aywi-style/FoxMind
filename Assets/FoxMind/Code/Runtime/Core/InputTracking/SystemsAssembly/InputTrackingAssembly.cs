using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.InputTracking.Systems;

namespace FoxMind.Code.Runtime.Core.InputTracking.SystemsAssembly
{
    public class InputTrackingAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new RegisterTrackingForComboSystem(),
                new TrackInputtedAttackSystem(),
                new TrackInputtedDashSystem(),
                new TrackInputtedMoveSystem(),
                new TrackInputtedJumpSystem(),
            };
        }
    }
}