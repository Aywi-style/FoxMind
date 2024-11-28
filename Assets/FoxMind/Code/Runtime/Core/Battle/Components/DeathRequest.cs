using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    public struct DeathRequest : IEntityFeature<DeathRequest>
    {
        public EcsPackedEntity For;
    }
}