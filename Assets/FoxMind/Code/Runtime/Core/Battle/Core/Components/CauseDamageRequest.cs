using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    public struct CauseDamageRequest : IEntityFeature<CauseDamageRequest>
    {
        public EcsPackedEntityWithWorld From;
        public EcsPackedEntityWithWorld To;
        public int BaseDamage;
        public float BaseCritChance;
        public float BaseCritMultiplier;
        public HitReactionType HitReactionType;
        public UnityEngine.Vector2 ReactionVelocity;
        public float HitReactionDuration;
        public int FinalDamage;
        public int StabilizationDamage;
    }
}
