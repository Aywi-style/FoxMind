using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Moving.Components
{
    public struct MotionAnimationComp : IEntityFeature<MotionAnimationComp>
    {
        [SerializeReference] public ITransition Move;
        [ReadOnly] public MixerState<Vector2> MoveState;
    }
}