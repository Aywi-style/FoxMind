using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Animations.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Animations.SystemsAssembly
{
    public class AnimationsAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>()
            {
                new PlayAnimationSystem(),
                new SelfPlayAnimationSystem(),
                new DelSelfPlayRequest(),
            };
        }
    }
}