using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    [Serializable]
    [Title("Feature: TransformComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct TransformComp : IEntityFeature<TransformComp>
    {
        public Transform Value;
        
        /*public void SetComposeValues(ref TransformComp component)
        {
            component.Value = Value;
        }*/
    }
}
