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
        private ComboConfig_v2 _cachedActualConfig;
        
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
                    float previousActionLastPress = Single.MinValue;
                    float currentActionLastPress = Single.MinValue;

                    // Проверка всех условий комбо атаки
                    foreach (var comboCondition in comboConfig.PlayerActions)
                    {
                        if (IsPassedCondition(comboCondition, requestedComboAttackEntity, comboConfig.LeadTime) == false)
                        {
                            isPassedAllConditions = false;
                            break;
                        }

                        if (comboConfig.OrderIsImportant)
                        {
                            currentActionLastPress = GetLastPress(comboCondition, requestedComboAttackEntity);

                            if (previousActionLastPress < currentActionLastPress)
                            {
                                previousActionLastPress = currentActionLastPress;
                            }
                            else
                            {
                                isPassedAllConditions = false;
                                break;
                            }
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
        
        private bool IsPassedCondition(PlayerAction playerAction, int entity, float leadTime)
        {
            return _cachedTime - GetLastPress(playerAction, entity) < leadTime;
        }

        private float GetLastPress(PlayerAction playerAction, int entity)
        {
            switch (playerAction)
            {
                case PlayerAction.Attack:
                    return _inputtedAttackPool.Value.Get(entity).LastPress;
                /*case PlayerAction.DoubleAttack:
                    return _inputtedAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.LongAttack:
                    return _inputtedAttackPool.Value.Get(entity).LastPress;*/
                case PlayerAction.Dash:
                    return _inputtedDashPool.Value.Get(entity).LastPress;
                case PlayerAction.Jump:
                    return _inputtedJumpPool.Value.Get(entity).LastPress;
                case PlayerAction.ForwardMove:
                    return _inputtedForwardMovePool.Value.Get(entity).LastPress;
                case PlayerAction.BackwardMove:
                    return _inputtedBackwardMovePool.Value.Get(entity).LastPress;
                case PlayerAction.LeftMove:
                    return _inputtedLeftMovePool.Value.Get(entity).LastPress;
                case PlayerAction.RightMove:
                    return _inputtedRightMovePool.Value.Get(entity).LastPress;
                default:
                    return Single.MinValue;
            }
        }
    }
}