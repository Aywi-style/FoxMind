using Sirenix.OdinInspector;
using UnityEngine;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Battle.Core.Enums;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Configs
{
    [CreateAssetMenu(fileName = "Attack_Name_№", menuName = "Configs/AttackConfig")]
    public class AttackConfig : SerializedScriptableObject
    {
        [field: FoldoutGroup("Attack Animation"), LabelText("Preview"), InlineEditor(InlineEditorModes.LargePreview), SerializeField, PropertyOrder(0)]
        public AnimationClip AttackAnimation { get; private set; }
        [FoldoutGroup("Attack Animation"), LabelText("Settings"), ShowInInspector, InlineEditor(InlineEditorModes.GUIAndHeader), PropertyOrder(0)]
        private AnimationClip AttackAnimationPreview
        {
            set => AttackAnimation = value;
            get => AttackAnimation;
        }
        
        [field: SerializeField] public int BaseDamage { private set; get; }
        [field: SerializeField] public float BaseCritChance { private set; get; }
        [field: SerializeField] public float BaseCritMultiplier { private set; get; }

        [field: Title("Animation Timing", bold: false), MinValue(0), SerializeField]
        public float AnimationDurationSeconds { private set; get; }

        [field: FoldoutGroup("Attacker Movement"), HideLabel, SerializeField]
        public AttackMovementSettings AttackerMovement { private set; get; }

        [field: Title("Hit Reaction", bold: false), SerializeField]
        public HitReactionFlags HitReactionFlags { private set; get; }
        
        [field: SerializeField] public Vector2 ReactionVelocity { private set; get; }
        [field: MinValue(0), SerializeField] public float HitReactionDuration { private set; get; } = 0.35f;
        
        [field: Title("End Of Continuous Part", bold: false), HideLabel, PropertyRange(0, 1), SerializeField, PropertyOrder(0)]
        public float EndOfContinuousPart { get; private set; }

        [Title("Hit Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(2)]
        public Vector2 HitWindow;
        
        [Title("Combo Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 ComboWindow;

        private float GetBaseAnimationDuration()
        {
            if (AnimationDurationSeconds > 0f)
            {
                return AnimationDurationSeconds;
            }

            return AttackAnimation != null ? AttackAnimation.length : 0f;
        }

        public float GetEffectiveAnimationDuration(float attackSpeed)
        {
            return GetBaseAnimationDuration() / Mathf.Max(0.01f, attackSpeed);
        }
    }
}
