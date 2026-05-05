using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    [Serializable]
    [Title("Feature: TargetFindableComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct TargetFindableComp : IEntityFeature<TargetFindableComp>
    {
        [Tooltip("Optional point used for lock-on direction and score. Entity TransformComp is used when empty.")]
        public Transform TargetPoint;
        
        [Tooltip("Additional designer priority for target selection.")]
        public float Priority;
    }
}
