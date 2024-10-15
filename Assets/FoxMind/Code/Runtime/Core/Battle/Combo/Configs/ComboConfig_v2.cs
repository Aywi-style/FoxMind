using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "ComboConfig_v2", menuName = "Configs/ComboConfig_v2")]
    [InlineEditor]
    public class ComboConfig_v2 : SerializedScriptableObject
    {
        private const string c_conditions = "Conditions"; 
        
        [SerializeField] public bool IsOpener;
        [SerializeField] public AttackConfig AttackConfig;
        [FoldoutGroup(c_conditions), HorizontalGroup(c_conditions + "/Horizontal"), SerializeField] public bool OrderIsImportant = true;
        [FoldoutGroup(c_conditions), HorizontalGroup(c_conditions + "/Horizontal"), SerializeField] public float LeadTime;
        [FoldoutGroup(c_conditions), SerializeField] public List<PlayerAction> PlayerActions;
        [SerializeField] public List<ComboConfig_v2> NextCombos;
    }
}