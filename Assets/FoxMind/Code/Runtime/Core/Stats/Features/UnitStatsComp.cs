using System;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Stats.Features
{
    [Serializable]
    public struct UnitStatsComp : IEntityFeature<UnitStatsComp>
    {
        public int EnergyMax;
        public int EnergyCurrent;
        [ReadOnly] public int EnergyReserved;

        public int Armor;
        public int ShieldMax;
        public int ShieldCurrent;
        public int BarrierMax;
        public int BarrierCurrent;

        /// <summary>
        /// Энергоэффективность
        /// </summary>
        public int EnergyEfficiency;
        /// <summary>
        /// Тактовая частота
        /// </summary>
        public int ClockSpeed;

        /// <summary>
        /// Множитель скорости проигрывания атак. 1 - базовая скорость, 0.5 - в два раза медленнее, 2 - в два раза быстрее.
        /// </summary>
        public float AttackSpeed;

        /// <summary>
        /// Охлаждение
        /// </summary>
        public int Cooling;
        /// <summary>
        /// Вместимость
        /// </summary>
        public int Capacity;
        /// <summary>
        /// Стабилизация, которую атакующий использует для расчёта шанса крита.
        /// </summary>
        public int Stabilization;

        /// <summary>
        /// Максимальный запас боевой стабилизации. Когда текущая стабилизация падает до нуля, юнит получает hit reaction.
        /// </summary>
        public float StabilizationMax;

        /// <summary>
        /// Текущий запас боевой стабилизации.
        /// </summary>
        public float StabilizationCurrent;

        /// <summary>
        /// Последнее время, когда юнит получил урон по боевой стабилизации.
        /// </summary>
        [ReadOnly] public float LastStabilizationDamageTime;

        /// <summary>
        /// Задержка перед началом восстановления стабилизации после последнего урона по ней.
        /// </summary>
        public float StabilizationRecoveryDelay;

        /// <summary>
        /// Время полного восстановления стабилизации от нуля до максимума.
        /// </summary>
        public float StabilizationRecoveryTime;

        /// <summary>
        /// Какие hit reaction эффекты разрешены для этого юнита.
        /// </summary>
        public HitReactionFlags AllowedHitReactions;

        /// <summary>
        /// Точка, от которой система считает направление реакции на удар. Если не задана, используется Transform сущности.
        /// </summary>
        public Transform ReactionCenter;

        public void SetComposeValues(ref UnitStatsComp component)
        {
            component = this;
            component.AttackSpeed = component.AttackSpeed > 0f ? component.AttackSpeed : 1f;
            component.StabilizationRecoveryDelay = component.StabilizationRecoveryDelay > 0f ? component.StabilizationRecoveryDelay : 0.5f;
            component.StabilizationRecoveryTime = component.StabilizationRecoveryTime > 0f ? component.StabilizationRecoveryTime : 0.5f;
            component.StabilizationCurrent = component.StabilizationMax > 0f && component.StabilizationCurrent <= 0f
                ? component.StabilizationMax
                : component.StabilizationCurrent;
        }
    }
}

