using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Fractions.Enums;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Fractions.Components
{
    [Serializable]
    [Title("Feature: FractionComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct FractionComp : IEntityFeature<FractionComp>
    {
        public FractionEnum Fraction;
    }
}
