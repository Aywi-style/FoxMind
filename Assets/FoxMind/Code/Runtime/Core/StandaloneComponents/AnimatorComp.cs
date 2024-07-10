using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    public struct AnimatorComp : IEntityFeature<AnimatorComp>
    {
        public Animator Value;
    }
}