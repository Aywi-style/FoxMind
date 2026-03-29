namespace FoxMind.Code.Runtime.Core.InputTracking.Components
{
    public struct RangeInputStateComp
    {
        public bool IsPressed;
        public bool LongTriggered;
        public bool PendingSingle;
        public float PressStartTime;
        public float PendingSingleTime;
        public float LastTapTime;
    }
}
