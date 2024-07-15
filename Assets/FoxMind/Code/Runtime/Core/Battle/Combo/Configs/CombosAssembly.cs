using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "CombosAssembly", menuName = "Configs/CombosAssembly")]
    [InlineEditor]
    public class CombosAssembly : SerializedScriptableObject
    {
        [SerializeField] public List<ComboConfig> ComboConfigs;
    }
}