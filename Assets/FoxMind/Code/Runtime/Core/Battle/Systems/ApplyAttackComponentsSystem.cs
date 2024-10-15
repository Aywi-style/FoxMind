using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    public class ApplyAttackComponentsSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;

        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;

        private float _cachedDeltaTime;
        private float _cachedTime;
        
        private float _currentNormalizedDeltaTime;
        private float _currentNormalizedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            _cachedDeltaTime = Time.deltaTime;

            if (_inAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (var inAttackEntity in _inAttackFilter.Value)
            {
                ref var inAttackComponent = ref _inAttackPool.Value.Get(inAttackEntity);

                _currentNormalizedTime = (_cachedTime - inAttackComponent.Start) / inAttackComponent.AttackConfig.AttackAnimation.length;
                _currentNormalizedDeltaTime = _cachedDeltaTime / inAttackComponent.AttackConfig.AttackAnimation.length;

                /*foreach (var timingComponent in inAttackComponent.AttackConfig.TimingComponents)
                {
                    bool isWindowForComponent = timingComponent.Time <= _currentNormalizedTime
                                                && timingComponent.Time >= (_currentNormalizedTime - _currentNormalizedDeltaTime);
                    
                    if (isWindowForComponent)
                    {
                        timingComponent.Component.Compose(_world.Value, inAttackEntity);
                    }
                }*/
            }
        }
    }
}