using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Battle.Configs
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/AttackConfig")]
    [InlineEditor]
    public class AttackConfig : SerializedScriptableObject
    {
        [Range(0, 1), SerializeField] public float AttackEnd = 1;
        [SerializeReference] public List<IEntityFeature> AttackEndComponents = new List<IEntityFeature>();
        [SerializeField] public List<TimingComponent> TimingComponents = new List<TimingComponent>();
        [SerializeReference] public AnimationClip Animation;

        private void OnValidate()
        {
            if (TimingComponents.Count < 2)
            {
                return;
            }
            
            for (int i = 0; i < TimingComponents.Count - 1; i++)
            {
                for (int j = i + 1; j < TimingComponents.Count; j++)
                {
                    if (TimingComponents[i].Time > TimingComponents[j].Time)
                        (TimingComponents[i], TimingComponents[j]) =(TimingComponents[j], TimingComponents[i]);
                }
            }
        }
    }

    [Serializable]
    public class TimingComponent
    {
        [field: Range(0, 1)] [field: SerializeField] public float Time { set; get; }
        [field: SerializeReference] public IEntityFeature Component { set; get; }
    }
}