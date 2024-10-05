using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    public struct MotionAnimationComp : IEntityFeature<MotionAnimationComp>
    {
        public TransitionAsset Move;
        [ReadOnly] public MixerState<Vector2> MoveState;
        //[ReadOnly] public AnimancerState MoveState;
    }
}