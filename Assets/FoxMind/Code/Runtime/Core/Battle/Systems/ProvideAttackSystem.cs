using System.Collections.Generic;
using Animancer;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.Components;
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
        private readonly EcsPoolInject<InAttackOveringComp> _inAttackOveringPool = default;
        private readonly EcsPoolInject<WeaponComp> _weaponPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        private readonly EcsPoolInject<SelfImmovableBecauseInAttackRequest> _selfImmovableBecauseInAttackRequestPool = default;

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
                
                if (targetProvideAttackRequest.PackedEntity.Unpack(_world.Value, out int targetEntity) == false)
                {
                    continue;
                }
                
                if (_inAttackPool.Value.Has(targetEntity) == false)
                {
                    _inAttackPool.Value.Add(targetEntity);
                }
                
                if (_inAttackOveringPool.Value.Has(targetEntity) == false)
                {
                    _inAttackOveringPool.Value.Add(targetEntity);
                }

                if (_selfImmovableBecauseInAttackRequestPool.Value.Has(targetEntity) == false)
                {
                    _selfImmovableBecauseInAttackRequestPool.Value.Add(targetEntity);
                }
                
                ref var inAttackComp = ref _inAttackPool.Value.Get(targetEntity);

                inAttackComp.AttackConfig = targetProvideAttackRequest.AttackConfig;
                inAttackComp.Start = _cachedTime;
                inAttackComp.End = _cachedTime + (targetProvideAttackRequest.AttackConfig.EndOfContinuousPart * inAttackComp.AttackConfig.AttackAnimation.length);
                
                ref var inAttackOveringComp = ref _inAttackOveringPool.Value.Get(targetEntity);
                inAttackOveringComp.Start = _cachedTime;
                inAttackOveringComp.End = _cachedTime + inAttackComp.AttackConfig.AttackAnimation.length;
                
                if (_animancerPool.Value.Has(targetEntity))
                {
                    ref var animancerComp = ref _animancerPool.Value.Get(targetEntity);
                    var state = animancerComp.Value.Play(targetProvideAttackRequest.AttackConfig.AttackAnimation, 0.2f);
                    state.Time = 0;
                    animancerComp.Value.Animator.applyRootMotion = true;
                }
            }
        }
    }
}