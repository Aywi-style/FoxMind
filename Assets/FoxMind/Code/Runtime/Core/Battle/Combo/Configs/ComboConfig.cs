using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Input.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "ComboConfig", menuName = "Configs/ComboConfig")]
    [InlineEditor]
    public class ComboConfig : SerializedScriptableObject
    {
        [SerializeField] public List<ComboCondition> ComboConditions;
        [SerializeField] public AttackConfig AttackConfig;
    }
}