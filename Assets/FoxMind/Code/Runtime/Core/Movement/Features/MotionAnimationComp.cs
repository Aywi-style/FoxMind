using System;
using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    [Serializable]
    [Title("Feature: MotionAnimationComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct MotionAnimationComp : IEntityFeature<MotionAnimationComp>
    {
        public TransitionAsset Move;
        [ReadOnly] public MixerState<Vector2> MoveState;
        //[ReadOnly] public AnimancerState MoveState;
    }
}
