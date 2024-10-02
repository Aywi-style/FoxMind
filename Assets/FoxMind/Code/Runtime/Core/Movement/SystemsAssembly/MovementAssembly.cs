using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.Systems;

namespace FoxMind.Code.Runtime.Core.Movement.SystemsAssembly
{
    public class MovementAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new DeletingImmovableCompSystem(),
                new AddingImmovableCompSystem(),
                new RegisterMotionAnimationsSystem(),
                new MoveSystem(),
                new JumpSystem(),
                new JumpCooldownSystem(),
                new MoveAnimationSystem(),
                new NewMoveAnimationSystem(),

                new DelJumpRequestSystem(),
                new DelRegisterMotionAnimationRequestSystem(),
                new DelSelfImmovableRequestSystem(),
                new DelSelfUnImmovableRequestSystem(),
            };
        }
    }
}