using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "CombosAssembly", menuName = "Configs/CombosAssembly")]
    [InlineEditor]
    public class CombosAssembly : SerializedScriptableObject
    {
        [SerializeField, ReadOnly] public List<ComboConfig> OpenerCombosConfigs_v2;
        [SerializeField, ReadOnly] public List<ComboConfig> NonOpenerCombosConfigs_v2;
        [SerializeField] private List<ComboConfig> ComboConfigs_v2;

        private void OnValidate()
        {
            OpenerCombosConfigs_v2.Clear();
            NonOpenerCombosConfigs_v2.Clear();
            
            foreach (var comboConfig in ComboConfigs_v2)
            {
                if (comboConfig.IsOpener)
                {
                    OpenerCombosConfigs_v2.Add(comboConfig);
                }
                else
                {
                    NonOpenerCombosConfigs_v2.Add(comboConfig);
                }
            }
        }
    }
}