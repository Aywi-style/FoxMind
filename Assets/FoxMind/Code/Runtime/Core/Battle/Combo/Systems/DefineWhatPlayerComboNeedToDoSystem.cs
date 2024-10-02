using System;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class DefineWhatPlayerComboNeedToDoSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<PlayerControlledComp, SelfDefineWhatComboNeedToDoRequest, CombinableComp>> _requestedComboAttackFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combosPool = default;
        
        private readonly EcsPoolInject<InputtedAttackComp> _inputtedAttackPool = default;
        private readonly EcsPoolInject<InputtedDashComp> _inputtedDashPool = default;
        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;
        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;
        
        private readonly EcsPoolInject<TargetProvideComboRequest> _targetProvideComboRequestPool = default;
        
        private float _cachedTime;
        private int _cachedConditions;
        private ComboConfig _cachedActualConfig;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            foreach (var requestedComboAttackEntity in _requestedComboAttackFilter.Value)
            {
                ref var combinableComp = ref _combosPool.Value.Get(requestedComboAttackEntity);
                
                _cachedConditions = 0;
                _cachedActualConfig = null;
                
                // Проходимся по всем доступным комбо
                foreach (var comboConfig in combinableComp.AvailableCombos)
                {
                    var maxConditions = 0;
                    bool isPassedAllConditions = true;
                    
                    // Проверка всех условий комбо атаки
                    foreach (var comboCondition in comboConfig.ComboConditions)
                    {
                        if (IsPassedCondition(comboCondition.Condition, requestedComboAttackEntity, comboCondition.PressWindow) == false)
                        {
                            isPassedAllConditions = false;
                            break;
                        }
                        
                        maxConditions++;
                    }

                    if (isPassedAllConditions == false)
                    {
                        continue;
                    }

                    if (maxConditions > _cachedConditions)
                    {
                        _cachedConditions = maxConditions;
                        _cachedActualConfig = comboConfig;
                    }
                }

                if (_cachedActualConfig != null)
                {
                    ref var targetProvideAttackRequest = ref _targetProvideComboRequestPool.Value.Add(_world.Value.NewEntity());
                    targetProvideAttackRequest.ComboConfig = _cachedActualConfig;
                    targetProvideAttackRequest.PackedEntity = _world.Value.PackEntity(requestedComboAttackEntity);
                }
            }
        }
        
        private bool IsPassedCondition(PlayerAction playerAction, int entity, float pressWindow)
        {
            switch (playerAction)
            {
                case PlayerAction.Attack:
                    return AttackPredicate(entity, pressWindow);
                case PlayerAction.Dash:
                    return DashPredicate(entity, pressWindow);
                case PlayerAction.Jump:
                    return JumpPredicate(entity, pressWindow);
                case PlayerAction.ForwardMove:
                    return ForwardMovePredicate(entity, pressWindow);
                case PlayerAction.BackwardMove:
                    return BackwardMovePredicate(entity, pressWindow);
                case PlayerAction.LeftMove:
                    return LeftMovePredicate(entity, pressWindow);
                case PlayerAction.RightMove:
                    return RightMovePredicate(entity, pressWindow);
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerAction), playerAction, null);
            }
        }

        private bool AttackPredicate(int entity, float pressWindow)
        {
            var lastPress = _inputtedAttackPool.Value.Get(entity).LastPress;
            
            return _cachedTime - lastPress < pressWindow;
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