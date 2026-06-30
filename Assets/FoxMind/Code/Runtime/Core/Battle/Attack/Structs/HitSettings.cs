using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Structs
{
    [Serializable]
    public struct HitSettings
    {
        [field: SerializeField] public HitDetectionMode DetectionMode { private set; get; }

        [field: Title("Hit Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true)]
        public Vector2 Window { private set; get; }

        [field: SerializeField] public HitReactionSettings ReactionSettings { private set; get; }
    }
}