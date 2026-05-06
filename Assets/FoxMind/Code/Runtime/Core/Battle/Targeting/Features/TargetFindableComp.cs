using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    /// <summary>
    /// Дизайнерский feature-компонент, который помечает сущность как доступную для soft/hard таргетинга.
    /// TargetPoint задаёт точку наведения UI/поворота, Priority добавляет ручной вес при выборе цели.
    /// </summary>
    [Serializable]
    [Title("Feature: TargetFindableComp")]
    [InfoBox("Дизайнерский feature-компонент. Добавляется через EntityBaker.Features или EntityTemplateConfig.")]
    public struct TargetFindableComp : IEntityFeature<TargetFindableComp>
    {
        [Tooltip("Опциональная точка для направления lock-on и UI-маркера. Если не задана, используется TransformComp сущности.")]
        public Transform TargetPoint;
        
        [Tooltip("Дополнительный дизайнерский приоритет при выборе цели.")]
        public float Priority;
    }
}
