using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using FoxMind.Code.Runtime.Core.Battle.Attack.Structs;
using Sirenix.OdinInspector;
using UnityEngine;
using FoxMind.Code.Runtime.Core.Battle.Components;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Configs
{
    [CreateAssetMenu(fileName = "Attack_Name_№", menuName = "Configs/AttackConfig")]
    public class AttackConfig : SerializedScriptableObject
    {
        private const string StatsGroupName = "Stats";
        private const string AnimationGroupName = "Animation";
        private const string ComboGroupName = "Combo";

        
        [field: SerializeField] public AttackRequirements Requirements { private set; get; }
        
        [field: FoldoutGroup(AnimationGroupName), PropertyRange(0, 1), SerializeField, PropertyOrder(0)]
        public float EndOfContinuousPart { get; private set; }
        
        [field: FoldoutGroup(AnimationGroupName), MinValue(0), SerializeField]
        public float OverrideAnimationDurationSeconds { private set; get; }
        
        [field: FoldoutGroup(AnimationGroupName), LabelText("Preview"), InlineEditor(InlineEditorModes.LargePreview), SerializeField, PropertyOrder(0)]
        public AnimationClip AttackAnimation { get; private set; }
        
        [FoldoutGroup(AnimationGroupName), LabelText("Settings"), ShowInInspector, InlineEditor(InlineEditorModes.GUIAndHeader), PropertyOrder(0)]
        private AnimationClip AttackAnimationPreview
        {
            set => AttackAnimation = value;
            get => AttackAnimation;
        }

        [field: FoldoutGroup(StatsGroupName), SerializeField] public AttackMovementMode RequiredMovementMode { private set; get; }
        [field: FoldoutGroup(StatsGroupName), SerializeField] public int BaseDamage { private set; get; }
        [field: FoldoutGroup(StatsGroupName), SerializeField] public float BaseCritChance { private set; get; }
        [field: FoldoutGroup(StatsGroupName), SerializeField] public float BaseCritMultiplier { private set; get; }

        [field: Title("Hit Reaction", bold: false), SerializeField]
        public HitReactionFlags HitReactionFlags { private set; get; }
        
        [field: SerializeField] public Vector2 ReactionVelocity { private set; get; }
        [field: MinValue(0), SerializeField] public float HitReactionDuration { private set; get; } = 0.35f;

        [Title("Hit Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(2)]
        public Vector2 HitWindow;
        
        [Title("Combo Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 ComboWindow;

        [field: SerializeField] public List<HitSettings> HitsSettings { private set; get; } = new List<HitSettings>();

        [field: FoldoutGroup(ComboGroupName), SerializeField] public bool IsComboPart { private set; get; }

        [field: FoldoutGroup(ComboGroupName), ShowIf(nameof(IsComboPart)), SerializeField]
        public List<ComboSettings> NextCombos { private set; get; } = new List<ComboSettings>();

        private float GetBaseAnimationDuration()
        {
            if (OverrideAnimationDurationSeconds > 0f)
            {
                return OverrideAnimationDurationSeconds;
            }

            return AttackAnimation != null ? AttackAnimation.length : 0f;
        }

        public float GetEffectiveAnimationDuration(float attackSpeed)
        {
            return GetBaseAnimationDuration() / Mathf.Max(0.01f, attackSpeed);
        }
    }
}
