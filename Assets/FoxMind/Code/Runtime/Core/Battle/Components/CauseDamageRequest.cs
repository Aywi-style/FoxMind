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
    }
}