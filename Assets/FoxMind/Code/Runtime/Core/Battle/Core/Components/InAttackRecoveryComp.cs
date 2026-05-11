using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Компонент, который показывает начало анимации атаки и её полный конец
    /// </summary>
    [Serializable]
    public struct InAttackRecoveryComp : IEntityFeature<InAttackRecoveryComp>
    {
        public AttackConfig AttackConfig;
        public float Start;
        public float End;
        public float AnimationDuration;
        public float AnimationSpeed;
    }
}
