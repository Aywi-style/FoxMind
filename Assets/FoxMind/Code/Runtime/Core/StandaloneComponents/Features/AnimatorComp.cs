using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    [Serializable]
    /*[Title("Feature: AnimatorComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]*/
    public struct AnimatorComp : IEntityFeature<AnimatorComp>
    {
        public Animator Value;
    }
}
