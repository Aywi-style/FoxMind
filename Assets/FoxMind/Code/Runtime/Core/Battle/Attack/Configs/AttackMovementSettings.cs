using System;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Configs
{
    /// <summary>
    /// Инспекторные настройки движения атакующего во время конкретной атаки.
    /// Не описывает реакцию цели: для цели используются HitReactionType/ReactionVelocity.
    /// </summary>
    [Serializable]
    public struct AttackMovementSettings
    {
        public AttackMovementMode Mode;
    }
}
