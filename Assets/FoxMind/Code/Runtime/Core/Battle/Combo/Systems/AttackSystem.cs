using System;
using System.Collections.Generic;
using Animancer;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class AttackSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, CombinableComp, AnimancerComp, MoveableComp>> _comboFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combosPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        
        private readonly EcsPoolInject<InputtedAttackComp> _inputtedAttackPool = default;
        private readonly EcsPoolInject<InputtedDashComp> _inputtedDashPool = default;
        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;
        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;

        private Dictionary<ComboState, IEcsPool>  _inputsPools;

        private float _cachedTime;
        
        public void Init(IEcsSystems systems)
        {
            _inputsPools = new Dictionary<ComboState, IEcsPool>()
                {
                    {
                        ComboState.Attack, _world.Value.GetPool<InputtedAttackComp>()
                    },
                    {
                        ComboState.Dash, _world.Value.GetPool<InputtedDashComp>()
                    },
                    {
                        ComboState.Jump, _world.Value.GetPool<InputtedJumpComp>()
                    },
                    {
                        ComboState.ForwardMove, _world.Value.GetPool<InputtedForwardMoveComp>()
                    },
                    {
                        ComboState.BackwardMove, _world.Value.GetPool<InputtedBackwardMoveComp>()
                    },
                    {
                        ComboState.LeftMove, _world.Value.GetPool<InputtedLeftMoveComp>()
                    },
                    {
                        ComboState.RightMove, _world.Value.GetPool<InputtedRightMoveComp>()
                    },
                }
                ;
        }

        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            // Берём сущность с комбо
            foreach (var combinableEntity in _comboFilter.Value)
            {
                ref var combinableComp = ref _combosPool.Value.Get(combinableEntity);

                // Проходимся по всем доступным комбо
                foreach (var comboConfig in combinableComp.CombosAssembly.ComboConfigs)
                {
                    bool isActualCombo = true;
                    
                    foreach (var comboCondition in comboConfig.ComboConditions)
                    {
                        if (IsPassedCondition(comboCondition.ComboState, combinableEntity, comboCondition.PressWindow))
                        {
                            continue;
                        }
                        
                        isActualCombo = false;
                        break;
                    }

                    if (isActualCombo == false)
                    {
                        continue;
                    }

                    ref var animancerComp = ref _animancerPool.Value.Get(combinableEntity);

                    SetComponentsOnEntity(comboConfig.AttackConfig.OpenerCompsForSource, combinableEntity);
                    animancerComp.Value.Animator.applyRootMotion = true;
                    var state = animancerComp.Value.Play(comboConfig.AttackConfig.Animation);
                    state.Time = 0;
                    state.Events.EndEvent = new AnimancerEvent(1, () => { SetComponentsOnEntity(comboConfig.AttackConfig.EndingCompsForSource, combinableEntity); });
                }
            }
        }

        private void SetComponentsOnEntity( List<IEntityFeature> attackComponents, int entity)
        {
            foreach (var IentityFeature in attackComponents)
            {
                IentityFeature.Compose(_world.Value, entity);
            }
        }
        
        private bool IsPassedCondition(ComboState comboState, int entity, float pressWindow)
        {
            switch (comboState)
            {
                case ComboState.Attack:
                    return AttackPredicate(entity, pressWindow);
                case ComboState.Dash:
                    return DashPredicate(entity, pressWindow);
                case ComboState.Jump:
                    return JumpPredicate(entity, pressWindow);
                case ComboState.ForwardMove:
                    return ForwardMovePredicate(entity, pressWindow);
                case ComboState.BackwardMove:
                    return BackwardMovePredicate(entity, pressWindow);
                case ComboState.LeftMove:
                    return LeftMovePredicate(entity, pressWindow);
                case ComboState.RightMove:
                    return RightMovePredicate(entity, pressWindow);
                default:
                    throw new ArgumentOutOfRangeException(nameof(comboState), comboState, null);
            }
        }

        private bool AttackPredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedAttackPool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool DashPredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedDashPool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool JumpPredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedJumpPool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool ForwardMovePredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedForwardMovePool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool BackwardMovePredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedBackwardMovePool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool LeftMovePredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedLeftMovePool.Value.Get(entity).LastPress < pressWindow;
        }

        private bool RightMovePredicate(int entity, float pressWindow)
        {
            return _cachedTime - _inputtedRightMovePool.Value.Get(entity).LastPress < pressWindow;
        }
    }
}