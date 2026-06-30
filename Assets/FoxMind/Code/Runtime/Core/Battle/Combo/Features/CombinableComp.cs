using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Features
{
    [Serializable]
    public struct CombinableComp : IEntityFeature<CombinableComp>
    {
        public CombosAssembly CombosAssembly;
        [ShowInInspector] [ReadOnly] public List<ComboConfig> AvailableCombos;
        public ComboConfig CurrentCombo;
        public ComboConfig NextCombo;
    }
}
