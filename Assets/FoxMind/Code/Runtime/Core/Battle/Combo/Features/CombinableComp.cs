using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Components
{
    [Serializable]
    [Title("Feature: CombinableComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct CombinableComp : IEntityFeature<CombinableComp>
    {
        public CombosAssembly CombosAssembly;
        [ReadOnly] public List<ComboConfig_v2> AvailableCombos;
    }
}
