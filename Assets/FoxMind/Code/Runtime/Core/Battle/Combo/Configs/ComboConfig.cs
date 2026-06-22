using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "Combo_Name_№", menuName = "Configs/ComboConfig")]
    [InlineEditor]
    public class ComboConfig : SerializedScriptableObject
    {
        private const string c_conditions = "Conditions";
        
        [SerializeField] public bool IsOpener;
        [SerializeField] public AttackConfig AttackConfig;
        [SerializeField] public ComboStanceCondition StanceCondition = ComboStanceCondition.Any;
        [FoldoutGroup(c_conditions), SerializeField] public List<ComboInput> MovementSetup = new ();
        [FoldoutGroup(c_conditions), SerializeField] public List<ComboInput> ComboActivators = new ();
        [SerializeField] public List<ComboConfig> NextCombos;

        public int GetPriority()
        {
            return MovementSetup.Count + ComboActivators.Count;
        }
    }
}
