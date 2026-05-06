namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Components
{
    /// <summary>
    /// Runtime-состояние кнопки таргетинга: хранит факт удержания, время нажатия и время последнего короткого клика.
    /// Нужно системам таргетинга для различения одиночного нажатия, двойного нажатия и удержания.
    /// </summary>
    public struct TargetingStateComp
    {
        public bool IsPressed;
        public float PressStartTime;
        public float LastTapTime;
    }
}
