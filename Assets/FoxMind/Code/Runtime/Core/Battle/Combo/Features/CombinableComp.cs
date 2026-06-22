using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Components
{
    [Serializable]
    public struct CombinableComp : IEntityFeature<CombinableComp>
    {
        public CombosAssembly CombosAssembly;
        [ShowInInspector] [ReadOnly] public TestClass AvailableCombos;
    }
}
