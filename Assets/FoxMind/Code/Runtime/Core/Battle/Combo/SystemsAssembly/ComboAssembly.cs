using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.SystemsAssembly
{
    public class ComboAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new AttackSystem()
            };
        }
    }
}