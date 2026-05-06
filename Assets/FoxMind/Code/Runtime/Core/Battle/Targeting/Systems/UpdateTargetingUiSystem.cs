using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Ui;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.Stats.Features;
using FoxMind.Code.Runtime.ProjectScope;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    /// <summary>
    /// Синхронизирует текущую hard target цель игрока с TargetingUi.
    /// Система читает ECS-состояние таргетинга и передаёт в UI Transform точки цели и снимок статов через R.TargetingUI.
    /// </summary>
    public class UpdateTargetingUiSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TargetingComp>> _targetingFilter = default;
        
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (R.TargetingUI == null)
            {
                return;
            }

            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                if (targeting.HasHardTarget == false
                    || targeting.HardTarget.Unpack(_world.Value, out var targetEntity) == false
                    || _transformPool.Value.Has(targetEntity) == false)
                {
                    TargetingUi.SetTarget(null, default);
                    continue;
                }

                var targetPoint = GetTargetPoint(targetEntity);
                var stats = _unitStatsPool.Value.Has(targetEntity)
                    ? _unitStatsPool.Value.Get(targetEntity)
                    : default;

                TargetingUi.SetTarget(targetPoint, stats);
            }
        }

        private Transform GetTargetPoint(int targetEntity)
        {
            if (_targetFindablePool.Value.Has(targetEntity))
            {
                ref var targetFindable = ref _targetFindablePool.Value.Get(targetEntity);
                if (targetFindable.TargetPoint != null)
                {
                    return targetFindable.TargetPoint;
                }
            }

            return _transformPool.Value.Get(targetEntity).Value;
        }
    }
}
