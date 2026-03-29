using FoxMind.Code.Runtime.Core.InputTracking.Configs;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking
{
    public static class CombatInputTuning
    {
        private const float c_defaultDoubleTap = 0.25f;
        private const float c_defaultLongPress = 0.40f;
        
        private static CombatInputSettings _settings;

        private static CombatInputSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<CombatInputSettings>("CombatInputSettings");
                }

                return _settings;
            }
        }

        public static float MeleeDoubleTapWindow => Settings != null ? Settings.MeleeDoubleTapWindow : c_defaultDoubleTap;
        public static float MeleeLongPressThreshold => Settings != null ? Settings.MeleeLongPressTime : c_defaultLongPress;
        
        public static float RangeDoubleTapWindow => Settings != null ? Settings.RangeDoubleTapWindow : c_defaultDoubleTap;
        public static float RangeLongPressThreshold => Settings != null ? Settings.RangeLongPressTime : c_defaultLongPress;
    }
}
