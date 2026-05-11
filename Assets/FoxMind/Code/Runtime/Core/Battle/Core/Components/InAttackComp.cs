using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    /// <summary>
    /// Компонент показывает какая атака воспроизводится, когда она началась и когда она может быть прервана
    /// </summary>
    /// <param name="Start">Старт воспроизводимой анимации</param>
    /// <param name="End">Время, когда можно преждевременно прервать атаку</param>
    public struct InAttackComp
    {
        public AttackConfig AttackConfig;
        public float Start;
        public float End;
        public float AnimationDuration;
        public float AnimationSpeed;
    }
}
