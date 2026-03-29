using System;
using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Animations.Components
{
    [Serializable]
    [Title("Feature: AnimancerComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct AnimancerComp : IEntityFeature<AnimancerComp>
    {
        public AnimancerComponent Value;
    }
}
