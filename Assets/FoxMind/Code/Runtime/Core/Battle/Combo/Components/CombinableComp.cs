using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Components
{
    public struct CombinableComp : IEntityFeature<CombinableComp>
    {
        public CombosAssembly CombosAssembly;
        public List<ComboConfig_v2> AvailableCombos;
    }
}