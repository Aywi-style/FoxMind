using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Structs
{
    [Serializable]
    public struct ComboSettings
    {
        [field: Title(nameof(ComboWindow), bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true)]
        public Vector2 ComboWindow { private set; get; }

        [field: SerializeField] public AttackConfig NextAttack { set; get; }
    }
}