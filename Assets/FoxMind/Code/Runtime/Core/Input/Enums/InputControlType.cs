namespace FoxMind.Code.Runtime.Core.Input.Enums
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
}