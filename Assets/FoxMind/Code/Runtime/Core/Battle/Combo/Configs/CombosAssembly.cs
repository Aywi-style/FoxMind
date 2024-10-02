using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "CombosAssembly", menuName = "Configs/CombosAssembly")]
    [InlineEditor]
    public class CombosAssembly : SerializedScriptableObject
    {
        [SerializeField] public List<ComboConfig> OpenerCombosConfigs;
        [SerializeField] public List<ComboConfig> NonOpenerCombosConfigs;
        [SerializeField] private List<ComboConfig> ComboConfigs;

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
        }
    }
}