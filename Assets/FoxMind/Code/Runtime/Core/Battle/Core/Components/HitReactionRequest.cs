using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Запрос на применение hit reaction к цели после пробития стабилизации.
    /// Сам по себе не двигает цель, а только переносит данные из атаки в ECS-логику реакции.
    /// </summary>
    public struct HitReactionRequest : IEntityFeature<HitReactionRequest>
    {
        public EcsPackedEntityWithWorld From;
        public EcsPackedEntityWithWorld To;
        public HitReactionFlags Type;
        public Vector2 ReactionVelocity;
        public float Duration;
    }
}
