using System.Collections.Generic;
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
                new CancelAttackOnModifierInputSystem(),
                new HitBoxEnablingSystem(),
                new HitBoxOverlapSystem(),
                new ApplyAttackComponentsSystem(),
                
                new CalculateFinalDamageSystem(),
                new CauseDamageSystem(),
                new TransitionFromAttackToAttackRecoverySystem(),
                new ExitFromAttackRecoverySystem(),
                
                new DeathSystem(),
                
                new DelTargetProvideAttackRequestSystem(),
                new DelCauseDamageRequestSystem(),
                new DelDeathRequestSystem()
            };
        }
    }
}
