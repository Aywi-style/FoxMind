namespace FoxMind.Code.Runtime.Core.Battle.Attack.Enums
{
    public enum HitDetectionMode
    {
        OncePerHit, // Один раз за всё время включения хёрт бокса
        OncePerTick, // Один раз за заданный интервал
        Continuous, // Эффект происходит всё время, пока цель находится внутри хёрт бокса
    }
}