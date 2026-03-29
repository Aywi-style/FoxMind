using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking.Configs
{
    [CreateAssetMenu(fileName = "CombatInputSettings", menuName = "Configs/CombatInputSettings")]
    public class CombatInputSettings : ScriptableObject
    {
        [Header("Double Tap Windows")]
        [Min(0f)] public float MeleeDoubleTapWindow = 0.25f;
        [Min(0f)] public float RangeDoubleTapWindow = 0.25f;
        
        [Header("Long Press Thresholds")]
        [Min(0f)] public float MeleeLongPressTime = 0.4f;
        [Min(0f)] public float RangeLongPressTime = 0.4f;
    }
}