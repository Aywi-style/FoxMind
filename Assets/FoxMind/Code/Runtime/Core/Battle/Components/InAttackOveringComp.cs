using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Компонент, который показывает начало анимации атаки и её полный конец
    /// </summary>
    [Serializable]
    public struct InAttackOveringComp : IEntityFeature<InAttackOveringComp>
    {
        public float Start;
        public float End;
    }
}