using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Statuses.Interfaces;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/AttackConfig")]
    [InlineEditor]
    public class AttackConfig : SerializedScriptableObject
    {
        [SerializeReference] public List<IStatusComponent> AttackEffects = new List<IStatusComponent>();
        [ReadOnly][SerializeReference] public List<IEntityFeature> EntityFeature = new List<IEntityFeature>();

#if UNITY_EDITOR
        private void OnValidate()
        {
            EntityFeature.Clear();

            foreach (var statusComponent in AttackEffects)
            {
                EntityFeature.Add((IEntityFeature)statusComponent);
            }
        }
#endif
    }
}