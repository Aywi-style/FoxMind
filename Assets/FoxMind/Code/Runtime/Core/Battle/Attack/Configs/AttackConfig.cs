using Sirenix.OdinInspector;
using UnityEngine;

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
        
        [field: SerializeField] public int DamageValue { private set; get; }
        
        [field: Title("End Of Continuous Part", bold: false), HideLabel, PropertyRange(0, 1), SerializeField, PropertyOrder(0)]
        public float EndOfContinuousPart { get; private set; }
        
        [HideLabel, ShowInInspector, ProgressBar(0, 1), PropertyOrder(1)]
        private float StackedHealthProgressBar => EndOfContinuousPart;

        [Title("Hit Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(2)]
        public Vector2 HitWindow;
        
        [Title("Combo Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 ComboWindow;

        [Title("Early Cancel Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 EarlyCancelWindow = new Vector2(0.15f, 0.45f);
        
        [Title("Late Cancel Window", bold: false), SerializeField, HideLabel, MinMaxSlider(0, 1, true), PropertyOrder(3)]
        public Vector2 LateCancelWindow = new Vector2(0.55f, 0.90f);
    }
}
