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
    public class ProvideAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<TargetProvideAttackRequest>> _targetProvideAttackRequestFilter = default;

        private readonly EcsPoolInject<TargetProvideAttackRequest> _targetProvideAttackRequestPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        private readonly EcsPoolInject<SelfImmovableRequest> _selfImmovableRequestPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_targetProvideAttackRequestFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var targetProvideAttackRequestEntity in _targetProvideAttackRequestFilter.Value)
            {
                ref var targetProvideAttackRequest = ref _targetProvideAttackRequestPool.Value.Get(targetProvideAttackRequestEntity);
                
                if (targetProvideAttackRequest.PackedEntity.Unpack(_world.Value, out int targetEntity) == false)
                {
                    continue;
                }
                
                if (_inAttackPool.Value.Has(targetEntity) == false)
                {
                    _inAttackPool.Value.Add(targetEntity);
                }

                if (_selfImmovableRequestPool.Value.Has(targetEntity) == false)
                {
                    _selfImmovableRequestPool.Value.Add(targetEntity);
                }
                
                ref var inAttackComp = ref _inAttackPool.Value.Get(targetEntity);

                inAttackComp.AttackConfig = targetProvideAttackRequest.AttackConfig;
                inAttackComp.Start = _cachedTime;
                inAttackComp.End = _cachedTime + (targetProvideAttackRequest.AttackConfig.AttackEnd * inAttackComp.AttackConfig.Animation.length);
                
                if (_animancerPool.Value.Has(targetEntity) == false)
                {
                    Debug.LogError($"Сущность {targetEntity} не имеет компонента анимации!");
                    continue;
                }
                ref var animancerComp = ref _animancerPool.Value.Get(targetEntity);
                var state = animancerComp.Value.Play(targetProvideAttackRequest.AttackConfig.Animation, 0.2f);
                state.Time = 0;
                animancerComp.Value.Animator.applyRootMotion = true;
                
                /*var attackConfig = targetProvideAttackRequest.AttackConfig;
                SetComponentsOnEntity(attackConfig.OpenerCompsForSource, targetEntity);
                if (state.EffectiveWeight == 0)
                {
                    ExitEvent.Register(state, () => Debug.Log("State Exited"));
                }

                foreach (var timeComponent in attackConfig.TimingComponents)
                {
                    state.Events.Add(new AnimancerEvent(timeComponent.Time,
                        () => { SetComponentsOnEntity(timeComponent.Component, targetEntity); }));
                }*/
                
                // Ваще не трогать
                //state.Events.EndEvent = new AnimancerEvent(1, () => { SetComponentsOnEntity(attackConfig.EndingCompsForSource, unpackedEntity); });
            }
        }

        /*private void SetComponentsOnEntity(IEntityFeature attackComponent, int entity)
        {
            attackComponent.Compose(_world.Value, entity);
        }

        private void SetComponentsOnEntity( List<IEntityFeature> attackComponents, int entity)
        {
            foreach (var IentityFeature in attackComponents)
            {
                IentityFeature.Compose(_world.Value, entity);
            }
        }*/
    }
}