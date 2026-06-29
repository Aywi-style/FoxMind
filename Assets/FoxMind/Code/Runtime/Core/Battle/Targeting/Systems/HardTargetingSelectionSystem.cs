using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Fractions.Components;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    /// <summary>
    /// Обрабатывает явный lock-on таргетинг игрока: первое нажатие выбирает hard target,
    /// повторные нажатия циклят цели по score, двойное нажатие сбрасывает hard target.
    /// </summary>
    public class HardTargetingSelectionSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, TargetingComp>> _targetingFilter = default;
        private readonly EcsFilterInject<Inc<TargetFindableComp, TransformComp>> _targetFindableFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;
        private readonly EcsPoolInject<FractionComp> _fractionPool = default;

        private readonly List<Candidate> _candidates = new List<Candidate>();
        
        private bool _hasDown = false;

        public void Run(IEcsSystems systems)
        {
            _hasDown = false;
            
            foreach (var inputEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputEntity);

                _hasDown = inputControlsComp.Value.GeneralMap.TargetLock.WasPressedThisFrame();
            }
            
            if (_hasDown == false)
            {
                return;
            }
            
            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                
                if (targeting.HasHardTarget && _hasDown)
                {
                    ClearHardTarget(ref targeting);
                    continue;
                }

                SelectOrCycleTarget(targetingEntity, ref targeting);
            }
        }

        private void SelectOrCycleTarget(int ownerEntity, ref TargetingComp targeting)
        {
            var unpacking = targeting.HardTarget.Unpack(_world.Value, out var currentTargetEntity);
            
            var isInitialSelection = targeting.HasHardTarget == false || unpacking == false;
            
            BuildCandidates(ownerEntity, ref targeting, isInitialSelection);
            if (_candidates.Count == 0)
            {
                ClearHardTarget(ref targeting);
                return;
            }

            _candidates.Sort(CompareCandidates);

            if (isInitialSelection)
            {
                SetHardTarget(ref targeting, _candidates[0]);
                return;
            }

            for (int i = 0; i < _candidates.Count; i++)
            {
                if (_candidates[i].Entity == currentTargetEntity)
                {
                    continue;
                }

                if (IsLowerThanCurrentTarget(_candidates[i], currentTargetEntity, targeting.HardTargetScore))
                {
                    SetHardTarget(ref targeting, _candidates[i]);
                    return;
                }
            }

            SetHardTarget(ref targeting, GetBestCandidateExcept(currentTargetEntity));
        }

        private void BuildCandidates(int ownerEntity, ref TargetingComp targeting, bool isInitialSelection)
        {
            _candidates.Clear();
            
            ref var ownerTransform = ref _transformPool.Value.Get(ownerEntity);
            var ownerPosition = ownerTransform.Value.position;
            var ownerForward = ownerTransform.Value.forward;
            ownerForward.y = 0f;
            
            if (ownerForward.sqrMagnitude <= float.Epsilon)
            {
                ownerForward = Vector3.forward;
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
                if (distance <= float.Epsilon || distance > targeting.HardSearchRadius)
                {
                    continue;
                }

                var direction = toTarget / distance;
                var angle = Vector3.Angle(ownerForward, direction);
                if (isInitialSelection && targeting.UseAngleForInitialHardTarget && angle > targeting.HardInitialSearchAngle)
                {
                    continue;
                }

                ref var findable = ref _targetFindablePool.Value.Get(targetEntity);
                var distanceScore = 1f - distance / targeting.HardSearchRadius;
                var score = distanceScore * targeting.HardDistanceWeight
                            + findable.Priority * targeting.HardPriorityWeight;
                
                _candidates.Add(new Candidate(targetEntity, score));
            }
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

        private int CompareCandidates(Candidate left, Candidate right)
        {
            var scoreComparison = right.Score.CompareTo(left.Score);
            if (scoreComparison != 0)
            {
                return scoreComparison;
            }

            return right.Entity.CompareTo(left.Entity);
        }

        private bool IsLowerThanCurrentTarget(Candidate candidate, int currentTargetEntity, float currentTargetScore)
        {
            const float c_scoreEpsilon = 0.0001f;

            if (candidate.Score < currentTargetScore - c_scoreEpsilon)
            {
                return true;
            }

            return Mathf.Abs(candidate.Score - currentTargetScore) <= c_scoreEpsilon
                   && candidate.Entity < currentTargetEntity;
        }

        private void SetHardTarget(ref TargetingComp targeting, Candidate candidate)
        {
            targeting.HasHardTarget = true;
            targeting.HardTarget = _world.Value.PackEntity(candidate.Entity);
            targeting.HardTargetScore = candidate.Score;
        }

        private Candidate GetBestCandidateExcept(int excludedEntity)
        {
            foreach (var candidate in _candidates)
            {
                if (candidate.Entity != excludedEntity)
                {
                    return candidate;
                }
            }

            return _candidates[0];
        }

        private void ClearHardTarget(ref TargetingComp targeting)
        {
            targeting.HasHardTarget = false;
            targeting.HardTarget = default;
            targeting.HardTargetScore = 0f;
            targeting.IsManualAiming = false;
            targeting.ManualAimDirection = Vector3.zero;
        }

        private readonly struct Candidate
        {
            public readonly int Entity;
            public readonly float Score;

            public Candidate(int entity, float score)
            {
                Entity = entity;
                Score = score;
            }
        }
    }
}
