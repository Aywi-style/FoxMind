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
    public class ValidateCurrentTargetSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, TargetingComp>> _targetingFilter = default;
        
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;
        private readonly EcsPoolInject<FractionComp> _fractionPool = default;
        private readonly EcsFilterInject<Inc<TargetFindableComp, TransformComp>> _targetFindableFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                if (targeting.HasHardTarget == false)
                {
                    continue;
                }

                if (targeting.HardTarget.Unpack(_world.Value, out var targetEntity) == false
                    || _targetFindablePool.Value.Has(targetEntity) == false
                    || _transformPool.Value.Has(targetEntity) == false
                    || IsOutOfRange(targetingEntity, targetEntity, ref targeting))
                {
                    TrySetNearestHardTarget(targetingEntity, ref targeting);
                }
            }
        }

        private bool IsOutOfRange(int ownerEntity, int targetEntity, ref TargetingComp targeting)
        {
            var ownerPosition = _transformPool.Value.Get(ownerEntity).Value.position;
            var targetPosition = GetTargetPosition(targetEntity);
            ownerPosition.y = 0f;
            targetPosition.y = 0f;

            return Vector3.Distance(ownerPosition, targetPosition) > targeting.HardSearchRadius;
        }

        private void TrySetNearestHardTarget(int ownerEntity, ref TargetingComp targeting)
        {
            var nearestEntity = -1;
            var nearestDistance = float.MaxValue;
            
            var ownerPosition = _transformPool.Value.Get(ownerEntity).Value.position;
            ownerPosition.y = 0f;

            foreach (var targetEntity in _targetFindableFilter.Value)
            {
                if (targetEntity == ownerEntity || IsSameFraction(ownerEntity, targetEntity))
                {
                    continue;
                }

                var targetPosition = GetTargetPosition(targetEntity);
                targetPosition.y = 0f;
                
                var distance = Vector3.Distance(ownerPosition, targetPosition);
                if (distance <= float.Epsilon || distance > targeting.HardSearchRadius || distance >= nearestDistance)
                {
                    continue;
                }

                nearestDistance = distance;
                nearestEntity = targetEntity;
            }

            if (nearestEntity < 0)
            {
                ClearHardTarget(ref targeting);
                return;
            }

            targeting.HasHardTarget = true;
            targeting.HardTarget = _world.Value.PackEntity(nearestEntity);
            targeting.HardTargetScore = GetHardTargetScore(ownerEntity, nearestEntity, ref targeting);
        }

        private float GetHardTargetScore(int ownerEntity, int targetEntity, ref TargetingComp targeting)
        {
            var ownerPosition = _transformPool.Value.Get(ownerEntity).Value.position;
            var targetPosition = GetTargetPosition(targetEntity);
            ownerPosition.y = 0f;
            targetPosition.y = 0f;

            var distance = Vector3.Distance(ownerPosition, targetPosition);
            var distanceScore = 1f - distance / targeting.HardSearchRadius;
            ref var findable = ref _targetFindablePool.Value.Get(targetEntity);

            return distanceScore * targeting.HardDistanceWeight
                   + findable.Priority * targeting.HardPriorityWeight;
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

        private void ClearHardTarget(ref TargetingComp targeting)
        {
            targeting.HasHardTarget = false;
            targeting.HardTarget = default;
            targeting.HardTargetScore = 0f;
        }
    }
}
