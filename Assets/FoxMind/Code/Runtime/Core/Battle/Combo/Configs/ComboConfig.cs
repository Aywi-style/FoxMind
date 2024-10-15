using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Battle.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "ComboConfig", menuName = "Configs/ComboConfig")]
    [InlineEditor]
    public class ComboConfig : SerializedScriptableObject
    {
        [SerializeField] public bool IsOpener;
        [SerializeField] public AttackConfig AttackConfig;
        [Range(0, 1)] [SerializeField] public float ComboEnd = 1f;
        [SerializeField] public List<ComboCondition> ComboConditions;
        [SerializeField] public AllowedCombos AllowedCombos;
    }

    [Serializable]
    public struct ComboCondition
    {
        [HorizontalGroup] public PlayerAction Condition;
        [HorizontalGroup] public float PressWindow;
    }

    [Serializable]
    public struct AllowedCombos
    {
        [Range(0, 1)] [SerializeField] public float LaunchWindow;
        [SerializeField] public List<ComboConfig> Combos;
    }
}