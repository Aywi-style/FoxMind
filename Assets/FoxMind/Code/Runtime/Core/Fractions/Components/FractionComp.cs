using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Fractions.Enums;

namespace FoxMind.Code.Runtime.Core.Fractions.Components
{
    public struct FractionComp : IEntityFeature<FractionComp>
    {
        public FractionEnum Fraction;
    }
}