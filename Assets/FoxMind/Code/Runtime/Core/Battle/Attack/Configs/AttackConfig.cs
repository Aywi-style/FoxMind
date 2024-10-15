using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Configs
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/AttackConfig")]
    public class AttackConfig : SerializedScriptableObject
    {
        [field: Title("End Of Continuous Part", bold: false), HideLabel, PropertyRange(0, 1), ShowInInspector, PropertyOrder(0)]
        public float EndOfContinuousPart { get; private set; }
        
        [HideLabel, ShowInInspector, ProgressBar(0, 1), PropertyOrder(1)]
        private float StackedHealthProgressBar => EndOfContinuousPart;

        [Title("Hit Window", bold: false), HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(2)]
        public Vector2 HitWindow;
        
        [Title("Combo Window", bold: false), HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 ComboWindow;
        
        [field: FoldoutGroup("Attack Animation"), LabelText("Preview"), InlineEditor(InlineEditorModes.LargePreview), SerializeField, PropertyOrder(4)]
        public AnimationClip AttackAnimation { get; private set; }
        [FoldoutGroup("Attack Animation"), LabelText("Settings"), ShowInInspector, InlineEditor(InlineEditorModes.GUIAndHeader), PropertyOrder(5)]
        private AnimationClip AttackAnimationPreview
        {
            set => AttackAnimation = value;
            get => AttackAnimation;
        }

        
        
        [field: FoldoutGroup("Return to Idle Animation"), LabelText("Preview"), InlineEditor(InlineEditorModes.LargePreview), SerializeField, PropertyOrder(4)]
        public AnimationClip ReturnToIdleAnimation { get; private set; }
        [FoldoutGroup("Return to Idle Animation"), LabelText("Settings"), ShowInInspector, InlineEditor(InlineEditorModes.GUIAndHeader), PropertyOrder(5)]
        private AnimationClip ReturnToIdleAnimationPreview
        {
            set => ReturnToIdleAnimation = value;
            get => ReturnToIdleAnimation;
        }
    }
}