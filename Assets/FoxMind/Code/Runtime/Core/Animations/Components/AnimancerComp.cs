using Animancer;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Animations.Components
{
    public struct AnimancerComp : IEntityFeature<AnimancerComp>
    {
        public AnimancerComponent Value;
    }
}