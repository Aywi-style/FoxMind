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
                new DeletingImmovableBecauseInAttackSystem(),
                new AddingImmovableCompSystem(),
                new AddingImmovableBecauseInAttackSystem(),
                new RegisterMoveableRequestSystem(),
                new RegisterMotionAnimationsSystem(),
                new UpdateInputsInSlayerJetControllerSystem(),
                new MoveSystem(),
                new JumpSystem(),
                new MoveAnimationSystem(),

                new DelJumpRequestSystem(),
                new DelRegisterMotionAnimationRequestSystem(),
                new DelRegisterMoveableRequestSystem(),
                new DelSelfImmovableRequestSystem(),
                new DelSelfImmovableBecauseInAttackRequestSystem(),
                new DelSelfUnImmovableRequestSystem(),
                new DelSelfUnImmovableBecauseInAttackRequestSystem(),
            };
        }
    }
}