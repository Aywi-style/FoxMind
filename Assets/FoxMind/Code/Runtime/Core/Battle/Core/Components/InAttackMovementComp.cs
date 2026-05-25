using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Runtime-состояние движения атакующего, созданное из AttackConfig.AttackerMovement.
    /// Живёт отдельно от InAttackComp/InAttackRecoveryComp, потому что movement window может иметь собственную длительность.
    /// </summary>
    public struct InAttackMovementComp
    {
        public AttackMovementMode Mode;
        public float StartTime;
        public float EndTime;
    }
}
