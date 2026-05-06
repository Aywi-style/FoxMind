using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    /// <summary>
    /// Дизайнерский feature-компонент с настройками и runtime-состоянием таргетинга сущности.
    /// Хранит отдельные параметры для soft targeting, hard targeting и manual aim.
    /// </summary>
    [Serializable]
    [Title("Feature: TargetingComp")]
    [InfoBox("Дизайнерские настройки таргетинга. Добавляется на сущности, которые могут выбирать цели.")]
    public struct TargetingComp : IEntityFeature<TargetingComp>
    {
        [Header("Soft Targeting")]
        [Min(0f)] public float SoftSearchRadius;
        [Range(0f, 180f)] public float SoftSearchAngle;
        public float SoftDistanceWeight;
        public float SoftAngleWeight;
        public float SoftPriorityWeight;
        
        [Header("Hard Targeting")]
        [Min(0f)] public float HardSearchRadius;
        public bool UseAngleForInitialHardTarget;
        [Range(0f, 180f)] public float HardInitialSearchAngle;
        public float HardDistanceWeight;
        public float HardPriorityWeight;
        
        [Header("Input")]
        [Min(0f)] public float DoubleTapWindow;
        [Min(0f)] public float HoldThreshold;
        [Range(0f, 1f)] public float RightStickDeadZone;
        
        [Header("Hard Target State")]
        [ReadOnly] public bool HasHardTarget;
        [ReadOnly] public EcsPackedEntity HardTarget;
        [ReadOnly] public float HardTargetScore;
        
        [Header("Soft Target State")]
        [ReadOnly] public bool HasSoftTarget;
        [ReadOnly] public EcsPackedEntity SoftTarget;
        [ReadOnly] public float SoftTargetScore;
        
        [Header("Manual Aim State")]
        [ReadOnly] public bool IsManualAiming;
        [ReadOnly] public Vector3 ManualAimDirection;

        public void SetComposeValues(ref TargetingComp component)
        {
            component = this;
            component.SoftSearchRadius = component.SoftSearchRadius > 0f ? component.SoftSearchRadius : 6f;
            component.SoftSearchAngle = component.SoftSearchAngle > 0f ? component.SoftSearchAngle : 70f;
            component.SoftDistanceWeight = component.SoftDistanceWeight > 0f ? component.SoftDistanceWeight : 1f;
            component.SoftAngleWeight = component.SoftAngleWeight > 0f ? component.SoftAngleWeight : 3f;
            component.SoftPriorityWeight = component.SoftPriorityWeight > 0f ? component.SoftPriorityWeight : 5f;
            
            component.HardSearchRadius = component.HardSearchRadius > 0f ? component.HardSearchRadius : 12f;
            component.UseAngleForInitialHardTarget = component.UseAngleForInitialHardTarget;
            component.HardInitialSearchAngle = component.HardInitialSearchAngle > 0f ? component.HardInitialSearchAngle : 120f;
            component.HardDistanceWeight = component.HardDistanceWeight > 0f ? component.HardDistanceWeight : 1f;
            component.HardPriorityWeight = component.HardPriorityWeight > 0f ? component.HardPriorityWeight : 5f;
            
            component.DoubleTapWindow = component.DoubleTapWindow > 0f ? component.DoubleTapWindow : 0.25f;
            component.HoldThreshold = component.HoldThreshold > 0f ? component.HoldThreshold : 0.25f;
            component.RightStickDeadZone = component.RightStickDeadZone > 0f ? component.RightStickDeadZone : 0.2f;
        }
    }
}
