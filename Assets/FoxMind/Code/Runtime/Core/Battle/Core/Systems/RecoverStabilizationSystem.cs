using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Восстанавливает боевую стабилизацию, если юнит на земле/в обычном состоянии и давно не получал урон по стабилизации.
    /// </summary>
    public class RecoverStabilizationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitStatsComp>, Exc<InHitReactionComp>> _unitStatsFilter = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitStatsFilter.Value)
            {
                ref var stats = ref _unitStatsPool.Value.Get(entity);
                if (stats.StabilizationMax <= 0f || stats.StabilizationCurrent >= stats.StabilizationMax)
                {
                    continue;
                }

                if (IsGrounded(entity) == false)
                {
                    continue;
                }

                if (Time.time < stats.LastStabilizationDamageTime + stats.StabilizationRecoveryDelay)
                {
                    continue;
                }

                var recoveryTime = Mathf.Max(0.01f, stats.StabilizationRecoveryTime);
                var recoveryPerSecond = stats.StabilizationMax / recoveryTime;
                stats.StabilizationCurrent = Mathf.Min(stats.StabilizationMax, stats.StabilizationCurrent + recoveryPerSecond * Time.deltaTime);
            }
        }

        private bool IsGrounded(int entity)
        {
            if (_characterControllerPool.Value.Has(entity) == false)
            {
                return true;
            }

            ref var characterController = ref _characterControllerPool.Value.Get(entity);
            return characterController.Value == null ||
                   characterController.Value.Motor == null ||
                   characterController.Value.Motor.GroundingStatus.IsStableOnGround;
        }
    }
}
