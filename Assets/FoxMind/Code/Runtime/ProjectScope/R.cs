using FoxMind.Code.Runtime.Core.Battle.Targeting.Ui;
using FoxMind.Code.Runtime.Core.Camera;
using FoxMind.Code.Runtime.Core.Ui;

namespace FoxMind.Code.Runtime.ProjectScope
{
    /// <summary>
    /// Resources
    /// </summary>
    public static class R
    {
        public static float TimeForReedAttackInput { get; set; } = 0.080f; // 0.083f
        public static float LastTimeForReedAttackInput { get; set; } = 0.150f; // 0.150f
        /// <summary>
        /// Отсчёт начинать от времени инпута атаки
        /// </summary>
        public static float LastTimeForReedMoveInput { get; set; } = 0.200f; // 0.150f
        public static float InputBufferWindow { get; set; } = 0.2f;
        
        public static CoreCamera CoreCamera;
        
        // UI
        public static CoreCanvas CoreCanvas;
        
        public static TargetingUi TargetingUI;
    }
}