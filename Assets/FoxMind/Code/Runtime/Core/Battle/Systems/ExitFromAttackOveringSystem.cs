using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    public class ExitFromAttackOveringSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackOveringComp>, Exc<InAttackComp>> _inAttackFilter = default;
        private readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;

        private readonly EcsPoolInject<InAttackOveringComp> _inAttackOveringPool = default;
        private readonly EcsPoolInject<SelfUnImmovableBecauseInAttackRequest> _selfUnImmovableBecauseInAttackRequestPool = default;
        private readonly EcsPoolInject<RegisterMotionAnimationRequest> _registerMotionAnimationRequestPool = default;
        private readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_inAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var inAttackEntity in _inAttackFilter.Value)
            {
                ref var inAttackOveringComp = ref _inAttackOveringPool.Value.Get(inAttackEntity);

                bool playerIsFreeze = true;
                foreach (var inputDirectionEntity in _inputDirectionFilter.Value)
                {
                    ref var inputDirectionComp = ref _inputDirectionPool.Value.Get(inputDirectionEntity);
                    
                    if (inputDirectionComp.Direction.x != 0 || inputDirectionComp.Direction.y != 0)
                    {
                        playerIsFreeze = false;
                        break;
                    }
                }

                if (_cachedTime < inAttackOveringComp.End && playerIsFreeze)
                {
                    continue;
                }
                /*foreach (var attackComponent in inAttackComponent.AttackConfig.AttackEndComponents)
                    {
                        attackComponent.Compose(_world.Value, inAttackEntity);
                    }*/

                _inAttackOveringPool.Value.Del(inAttackEntity);

                
                if (_selfUnImmovableBecauseInAttackRequestPool.Value.Has(inAttackEntity) == false)
                {
                    _selfUnImmovableBecauseInAttackRequestPool.Value.Add(inAttackEntity);
                }

                _registerMotionAnimationRequestPool.Value.Add(inAttackEntity);
            }
        }
    }
}