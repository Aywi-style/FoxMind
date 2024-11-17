using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    public struct InAttackOveringComp : IEntityFeature<InAttackOveringComp>
    {
        public float Start;
        public float End;
    }
}