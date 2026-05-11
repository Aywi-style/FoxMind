using Sirenix.OdinInspector;
using UnityEngine;
using FoxMind.Code.Runtime.Core.Battle.Components;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Configs
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/AttackConfig")]
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

        [field: Title("Hit Reaction", bold: false), SerializeField]
        public HitReactionType HitReactionType { private set; get; }
        
        [field: SerializeField]
        public Vector2 ReactionVelocity { private set; get; }

        [field: MinValue(0), SerializeField]
        public float HitReactionDuration { private set; get; } = 0.35f;
        
        [field: Title("End Of Continuous Part", bold: false), HideLabel, PropertyRange(0, 1), SerializeField, PropertyOrder(0)]
        public float EndOfContinuousPart { get; private set; }
        
        [HideLabel, ShowInInspector, ProgressBar(0, 1), PropertyOrder(1)]
        private float StackedHealthProgressBar => EndOfContinuousPart;

        [Title("Hit Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(2)]
        public Vector2 HitWindow;
        
        [Title("Combo Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 ComboWindow;

        public float GetBaseAnimationDuration()
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

        public float GetEffectiveAnimationSpeed(float attackSpeed)
        {
            var baseDuration = GetBaseAnimationDuration();
            if (AttackAnimation == null || AttackAnimation.length <= 0f || baseDuration <= 0f)
            {
                return Mathf.Max(0.01f, attackSpeed);
            }

            return AttackAnimation.length / baseDuration * Mathf.Max(0.01f, attackSpeed);
        }
    }
}
