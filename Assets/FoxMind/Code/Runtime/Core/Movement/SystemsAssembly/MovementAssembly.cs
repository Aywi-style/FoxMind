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
                new RemoveAttackMovementLockSystem(),
                new ApplyAttackMovementLockSystem(),
                new RegisterMoveableRequestSystem(),
                new RegisterMotionAnimationsSystem(),
                new UpdateCharacterControllerSystem(),
                new JumpSystem(),
                new MoveAnimationSystem(),
                new DelJumpRequestSystem(),
                new DelRegisterMoveableRequestSystem(),
                new DelAttackMovementLockRequestSystem(),
                new DelAttackMovementUnlockRequestSystem(),
            };
        }
    }
}
