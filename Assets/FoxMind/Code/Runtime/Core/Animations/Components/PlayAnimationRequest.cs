using Leopotam.EcsLite;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Animations.Components
{
    public struct PlayAnimationRequest
    {
        public EcsPackedEntity Target;
        public AnimationClip Clip;
        public bool IsRootMotion;
    }
}