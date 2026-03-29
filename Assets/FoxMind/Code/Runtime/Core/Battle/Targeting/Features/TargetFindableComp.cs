using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    [Serializable]
    [Title("Feature: TargetFindableComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct TargetFindableComp : IEntityFeature<TargetFindableComp>
    {
        public EcsPackedEntity Target;
    }
}
