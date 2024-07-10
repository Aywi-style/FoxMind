using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Moving.Systems;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Moving.SystemsAssembly
{
    public class MovementAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new RegisterMotionAnimationsSystem(),
                new MoveSystem(),
                new JumpSystem(),
                new MoveAnimationSystem(),
                new NewMoveAnimationSystem(),
            };
        }
    }
}