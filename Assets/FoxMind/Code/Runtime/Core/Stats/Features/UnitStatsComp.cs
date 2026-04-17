using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

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
        /// Охлаждение
        /// </summary>
        public int Cooling;
        /// <summary>
        /// Вместимость
        /// </summary>
        public int Capacity;
        /// <summary>
        /// Стабилизация
        /// </summary>
        public int Stabilization;
    }
}