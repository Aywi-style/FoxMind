using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Configs
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/AttackConfig")]
    [InlineEditor]
    public class AttackConfig : SerializedScriptableObject
    {
        [SerializeReference] public List<IEntityFeature> OpenerCompsForSource = new List<IEntityFeature>();
        [SerializeReference] public List<IEntityFeature> EndingCompsForSource = new List<IEntityFeature>();
        [SerializeReference] public List<IEntityFeature> HitCompsForTarget = new List<IEntityFeature>();
        [SerializeReference] public AnimationClip Animation;
    }
}