using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    [Serializable]
    public struct MoveableComp : IEntityFeature<MoveableComp>
    {
        [ReadOnly] public Vector3 NormalizedMoveDirection;
        public float Speed;
    }
}