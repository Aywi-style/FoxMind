using System;

namespace FoxMind.Code.Runtime.Core.Battle.Core.Enums
{
    /// <summary>
    /// Набор разрешённых hit reaction эффектов для юнита.
    /// Используется статами цели, чтобы запретить отдельные реакции вроде лаунча или кнокдауна.
    /// </summary>
    [Flags]
    public enum HitReactionFlags
    {
        None = 0,
        Stagger = 1 << 0,
        Knockback = 1 << 1,
        Knockdown = 1 << 2,
        Launch = 1 << 3,
        Juggle = 1 << 4,

        
        AttackBase = Stagger | Juggle,
        All = Stagger | Knockback | Knockdown | Launch | Juggle,
    }
}
