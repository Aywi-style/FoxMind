using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    public struct EnergyComp : IEntityFeature<EnergyComp>
    {
        public float MaxValue;
        public float CurrentValue;
    }
}