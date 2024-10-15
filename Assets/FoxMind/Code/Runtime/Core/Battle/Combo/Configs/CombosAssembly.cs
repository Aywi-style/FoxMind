using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "CombosAssembly", menuName = "Configs/CombosAssembly")]
    [InlineEditor]
    public class CombosAssembly : SerializedScriptableObject
    {
        [SerializeField, ReadOnly] public List<ComboConfig> OpenerCombosConfigs;
        [SerializeField, ReadOnly] public List<ComboConfig> NonOpenerCombosConfigs;
        [SerializeField] private List<ComboConfig> ComboConfigs;
        
        [SerializeField, ReadOnly] public List<ComboConfig_v2> OpenerCombosConfigs_v2;
        [SerializeField, ReadOnly] public List<ComboConfig_v2> NonOpenerCombosConfigs_v2;
        [SerializeField] private List<ComboConfig_v2> ComboConfigs_v2;

        private void OnValidate()
        {
            OpenerCombosConfigs.Clear();
            NonOpenerCombosConfigs.Clear();
            
            foreach (var comboConfig in ComboConfigs)
            {
                if (comboConfig.IsOpener)
                {
                    OpenerCombosConfigs.Add(comboConfig);
                }
                else
                {
                    NonOpenerCombosConfigs.Add(comboConfig);
                }
            }
            
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