using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    [Title("Feature: EnergyComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct EnergyComp : IEntityFeature<EnergyComp>
    {
        public float MaxValue;
        public float CurrentValue;
    }
}
