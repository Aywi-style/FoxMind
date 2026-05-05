using FoxMind.Code.Runtime.Core.Battle.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Title("Feature: HurtBoxCapsuleComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct HurtBoxComp : IEntityFeature<HurtBoxComp>
    {
        public HurtBoxMb Value;
    }
}
