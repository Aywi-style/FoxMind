namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Тип реакции цели на удар. None означает, что атака наносит урон, но не ломает поведение цели.
    /// </summary>
    public enum HitReactionType
    {
        None = 0,
        StaggerAndAirJuggle = 1,
        Knockback = 2,
        Launch = 3,
        Knockdown = 4,
    }
}
