using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Animations.Components
{
    public struct SelfPlayAnimationRequest : IEntityFeature<SelfPlayAnimationRequest>
    {
        public AnimationClip Clip;
        public bool IsRootMotion;
    }
}