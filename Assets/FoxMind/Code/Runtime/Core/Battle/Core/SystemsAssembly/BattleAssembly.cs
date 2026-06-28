using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Core.Systems;
using FoxMind.Code.Runtime.Core.Battle.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Battle.Core.SystemsAssembly
{
    public class BattleAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new ProvideAttackSystem(),
                new CancelAttackSystem(),
                new HitBoxEnablingSystem(),
                new HitBoxOverlapSystem(),
                
                new CalculateFinalDamageSystem(),
                new CauseDamageSystem(),
                new ApplyStabilizationDamageSystem(),
                new ApplyHitReactionRequestSystem(),
                new ApplyHitReactionMovementSystem(),
                new InHitReactionComponentDeletingSystem(),
                new RecoverStabilizationSystem(),
                // new CancelAttackSystem(),
                
                new DeathSystem(),
                
                new CleanUpAfterAttackSystem(),
                new DelTargetProvideAttackRequestSystem(),
                new DelCauseDamageRequestSystem(),
                new DelHitReactionRequestSystem(),
                new DelDeathRequestSystem()
            };
        }
    }
}
