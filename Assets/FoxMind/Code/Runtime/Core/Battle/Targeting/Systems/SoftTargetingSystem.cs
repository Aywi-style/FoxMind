using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Fractions.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    /// <summary>
    /// Постоянно ищет лучшую soft target цель перед игроком.
    /// Soft target используется как скрытый боевой ассист для доворачивания атак и не является явным lock-on.
    /// </summary>
    public class SoftTargetingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, TargetingComp>> _targetingFilter = default;
        private readonly EcsFilterInject<Inc<TargetFindableComp, TransformComp>> _targetFindableFilter = default;

        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;
        private readonly EcsPoolInject<FractionComp> _fractionPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                FindSoftTarget(targetingEntity, ref targeting);
            }
        }

        private void FindSoftTarget(int ownerEntity, ref TargetingComp targeting)
        {
            var bestEntity = -1;
            var bestScore = float.MinValue;
            
            ref var ownerTransform = ref _transformPool.Value.Get(ownerEntity);
            var ownerPosition = ownerTransform.Value.position;
            var ownerForward = ownerTransform.Value.forward;
            ownerForward.y = 0f;

            if (ownerForward.sqrMagnitude <= float.Epsilon)
            {
                ClearSoftTarget(ref targeting);
                return;
            }

            ownerForward.Normalize();

            foreach (var targetEntity in _targetFindableFilter.Value)
            {
                if (targetEntity == ownerEntity || IsSameFraction(ownerEntity, targetEntity))
                {
                    continue;
                }

                var targetPosition = GetTargetPosition(targetEntity);
                var toTarget = targetPosition - ownerPosition;
                toTarget.y = 0f;
                
                var distance = toTarget.magnitude;
                if (distance <= float.Epsilon || distance > targeting.SoftSearchRadius)
                {
                    continue;
                }

                var direction = toTarget / distance;
                var angle = Vector3.Angle(ownerForward, direction);
                if (angle > targeting.SoftSearchAngle)
                {
                    continue;
                }

                ref var findable = ref _targetFindablePool.Value.Get(targetEntity);
                var distanceScore = 1f - distance / targeting.SoftSearchRadius;
                var angleScore = 1f - angle / Mathf.Max(targeting.SoftSearchAngle, 1f);
                var score = distanceScore * targeting.SoftDistanceWeight
                            + angleScore * targeting.SoftAngleWeight
                            + findable.Priority * targeting.SoftPriorityWeight;

                if (score <= bestScore)
                {
                    continue;
                }

                bestScore = score;
                bestEntity = targetEntity;
            }

            if (bestEntity < 0)
            {
                ClearSoftTarget(ref targeting);
                return;
            }

            targeting.HasSoftTarget = true;
            targeting.SoftTarget = _world.Value.PackEntity(bestEntity);
            targeting.SoftTargetScore = bestScore;
        }

        private Vector3 GetTargetPosition(int targetEntity)
        {
            ref var findable = ref _targetFindablePool.Value.Get(targetEntity);
            if (findable.TargetPoint != null)
            {
                return findable.TargetPoint.position;
            }

            return _transformPool.Value.Get(targetEntity).Value.position;
        }

        private bool IsSameFraction(int ownerEntity, int targetEntity)
        {
            if (_fractionPool.Value.Has(ownerEntity) == false || _fractionPool.Value.Has(targetEntity) == false)
            {
                return false;
            }

            return _fractionPool.Value.Get(ownerEntity).Fraction == _fractionPool.Value.Get(targetEntity).Fraction;
        }

        private void ClearSoftTarget(ref TargetingComp targeting)
        {
            targeting.HasSoftTarget = false;
            targeting.SoftTarget = default;
            targeting.SoftTargetScore = 0f;
        }
    }
}
