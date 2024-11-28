using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Battle.SystemsAssembly
{
    public class BattleAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new ProvideAttackSystem(),
                new HitBoxEnablingSystem(),
                new ApplyAttackComponentsSystem(),
                
                new CauseDamageSystem(),
                new TransitionFromAttackToAttackOveringSystem(),
                new ExitFromAttackOveringSystem(),
                
                new DeathSystem(),
                
                new DelTargetProvideAttackRequestSystem(),
                new DelCauseDamageRequestSystem(),
                new DelDeathRequestSystem()
            };
        }
    }
}