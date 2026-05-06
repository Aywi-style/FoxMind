namespace FoxMind.Code.Runtime.Core.Input.Components
{
    /// <summary>
    /// Последний тип устройства, с которого пришёл игровой ввод.
    /// Нужен системам, где нельзя смешивать схемы управления, например manual targeting мышью и стиком.
    /// </summary>
    public enum InputControlType
    {
        Unknown,
        KeyboardMouse,
        Gamepad,
    }

    /// <summary>
    /// Глобальное ECS-состояние базового input asset и последнего активного типа управления.
    /// </summary>
    public struct BaseInputControlsComp
    {
        public BaseControls Value;
        public InputControlType ActiveControlType;
    }
}
