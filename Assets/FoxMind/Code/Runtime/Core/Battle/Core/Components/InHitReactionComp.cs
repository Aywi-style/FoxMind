using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Текущее состояние цели, которая находится под hit reaction эффектом.
    /// Пока этот компонент висит на юните, его стабилизация не восстанавливается.
    /// </summary>
    [Serializable]
    public struct InHitReactionComp : IEntityFeature<InHitReactionComp>
    {
        public HitReactionFlags Type;
        public Vector2 ReactionVelocity;
        public Vector3 WorldReactionVelocity;
        public float StartTime;
        public float Duration;
        public float EndTime;
        public bool WaitForGroundBeforeTimer;
    }
}
