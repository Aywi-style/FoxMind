using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    public struct AnimancerComp : IEntityFeature<AnimancerComp>
    {
        public AnimancerComponent Value;
        public AnimationClip Clip;
    }
}