using System.Collections.Generic;
using Animancer;
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Система, которая 
    /// </summary>
    public class ProvideAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<ProvideAttackRequest>> _provideAttackRequestFilter = default;

        private readonly EcsPoolInject<ProvideAttackRequest> _provideAttackRequestPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;
        private readonly EcsPoolInject<InAttackMovementComp> _inAttackMovementPool = default;
        private readonly EcsPoolInject<WeaponComp> _weaponPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;
        
        private readonly EcsPoolInject<AttackMovementLockRequest> _attackMovementLockRequestPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_provideAttackRequestFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var targetProvideAttackRequestEntity in _provideAttackRequestFilter.Value)
            {
                ref var targetProvideAttackRequest = ref _provideAttackRequestPool.Value.Get(targetProvideAttackRequestEntity);

                if (targetProvideAttackRequest.AttackConfig == null)
                {
                    continue;
                }
                
                if (targetProvideAttackRequest.PackedEntity.Unpack(_world.Value, out int targetEntity) == false)
                {
                    continue;
                }
                
                if (_inAttackPool.Value.Has(targetEntity) == false)
                {
                    _inAttackPool.Value.Add(targetEntity);
                }
                
                if (_inAttackRecoveryPool.Value.Has(targetEntity) == false)
                {
                    _inAttackRecoveryPool.Value.Add(targetEntity);
                }

                if (_attackMovementLockRequestPool.Value.Has(targetEntity) == false)
                {
                    _attackMovementLockRequestPool.Value.Add(targetEntity);
                }
                
                ref var inAttackComp = ref _inAttackPool.Value.Get(targetEntity);
                var attackSpeed = GetAttackSpeed(targetEntity);
                var animationDuration = targetProvideAttackRequest.AttackConfig.GetEffectiveAnimationDuration(attackSpeed);
                var animationSpeed = targetProvideAttackRequest.AttackConfig.GetEffectiveAnimationSpeed(attackSpeed);

                inAttackComp.AttackConfig = targetProvideAttackRequest.AttackConfig;
                inAttackComp.Start = _cachedTime;
                inAttackComp.AnimationDuration = animationDuration;
                inAttackComp.AnimationSpeed = animationSpeed;
                inAttackComp.End = _cachedTime + (targetProvideAttackRequest.AttackConfig.EndOfContinuousPart * animationDuration);
                
                ref var inAttackRecoveryComp = ref _inAttackRecoveryPool.Value.Get(targetEntity);
                inAttackRecoveryComp.AttackConfig = inAttackComp.AttackConfig;
                inAttackRecoveryComp.Start = _cachedTime;
                inAttackRecoveryComp.AnimationDuration = animationDuration;
                inAttackRecoveryComp.AnimationSpeed = animationSpeed;
                inAttackRecoveryComp.End = _cachedTime + animationDuration;

                SetupAttackMovement(targetEntity, targetProvideAttackRequest.AttackConfig, animationDuration);
                
                if (_animancerPool.Value.Has(targetEntity))
                {
                    ref var animancerComp = ref _animancerPool.Value.Get(targetEntity);
                    if (targetProvideAttackRequest.AttackConfig.AttackAnimation == null)
                    {
                        continue;
                    }

                    var state = animancerComp.Value.Play(targetProvideAttackRequest.AttackConfig.AttackAnimation, 0.2f);
                    state.Time = 0;
                    state.Speed = animationSpeed;
                    // state.Duration = 
                    animancerComp.Value.Animator.applyRootMotion = true;
                }
            }
        }

        private float GetAttackSpeed(int entity)
        {
            if (_unitStatsPool.Value.Has(entity) == false)
            {
                return 1f;
            }

            ref var unitStats = ref _unitStatsPool.Value.Get(entity);
            return unitStats.AttackSpeed > 0f ? unitStats.AttackSpeed : 1f;
        }

        private void SetupAttackMovement(int targetEntity, AttackConfig attackConfig, float animationDuration)
        {
            if (attackConfig.AttackerMovement.Mode == AttackMovementMode.None)
            {
                if (_inAttackMovementPool.Value.Has(targetEntity))
                {
                    _inAttackMovementPool.Value.Del(targetEntity);
                }

                return;
            }

            if (_inAttackMovementPool.Value.Has(targetEntity) == false)
            {
                _inAttackMovementPool.Value.Add(targetEntity);
            }

            ref var inAttackMovement = ref _inAttackMovementPool.Value.Get(targetEntity);
            inAttackMovement.Mode = attackConfig.AttackerMovement.Mode;
            inAttackMovement.StartTime = _cachedTime;
            inAttackMovement.EndTime = _cachedTime + attackConfig.GetAttackMovementEndNormalizedTime() * animationDuration;
        }
    }
}
