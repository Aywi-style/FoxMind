using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "ComboConfig", menuName = "Configs/ComboConfig")]
    [InlineEditor]
    public class ComboConfig : SerializedScriptableObject
    {
        [SerializeField] public bool IsRequiredPreviousAttack;
        [ShowIf("IsRequiredPreviousAttack")] [SerializeField] public PreviousAttack PreviousAttack;
        [SerializeField] public List<ComboCondition> ComboConditions;
        [SerializeField] public AttackConfig AttackConfig;
    }

    [Serializable]
    public struct ComboCondition
    {
        public ComboState ComboState;
        public float PressWindow;
    }

    [Serializable]
    public struct PreviousAttack
    {
        public AttackConfig Attack;
        public float LaunchedWindow;
    }
}