using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.Enums;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using JetBrains.Annotations;
using KinematicCharacterController;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Компонент, который отвечат за возможность сущности передвигаться за счёт управления
    /// Самостоятельно реализует метод Compose внутри себя
    /// </summary>
    [Serializable]
    [Title("Feature: MoveableComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct MoveableComp : IEntityFeature<MoveableComp>
    {
        public CustomCharacterController CustomCharacterController;
        public KinematicCharacterMotor Motor;
        [Sirenix.OdinInspector.ReadOnly] public Vector3 NormalizedMoveDirection;
        public float CurrentMaxSpeedFromMovementBehaviour;
    }
}
