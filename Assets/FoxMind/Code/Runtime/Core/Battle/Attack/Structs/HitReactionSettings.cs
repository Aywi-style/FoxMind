using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Structs
{
    [Serializable]
    public struct HitReactionSettings
    {
        [field: SerializeField] public HitReactionFlags Reaction { private set; get; }
        [field: SerializeField] public Vector2 Velocity { private set; get; }
        [field: MinValue(0), SerializeField] public float Duration { private set; get; }
    }
}