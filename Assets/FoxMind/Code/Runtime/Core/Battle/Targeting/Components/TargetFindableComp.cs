using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    [Serializable]
    public struct TargetFindableComp : IEntityFeature<TargetFindableComp>
    {
        public EcsPackedEntity Target;
    }
}