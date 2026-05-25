using System;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
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
        
        private readonly EcsPoolInject<InputtedMeleeAttackComp> _inputtedMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedRangeAttackComp> _inputtedRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleMeleeAttackComp> _inputtedDoubleMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongMeleeAttackComp> _inputtedLongMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleRangeAttackComp> _inputtedDoubleRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongRangeAttackComp> _inputtedLongRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedDashComp> _inputtedDashPool = default;
        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;
        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;
        
        private readonly EcsPoolInject<TargetProvideComboRequest> _targetProvideComboRequestPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;
        
        private float _cachedTime;
        private int _cachedConditions;
        private ComboConfig_v2 _cachedActualConfig;
        private int _cachedPriority;
        private float _cachedLastPress;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            foreach (var requestedComboAttackEntity in _requestedComboAttackFilter.Value)
            {
                ref var combinableComp = ref _combosPool.Value.Get(requestedComboAttackEntity);

                _cachedConditions = 0;
                _cachedActualConfig = null;
                _cachedPriority = int.MinValue;
                _cachedLastPress = Single.MinValue;
                
                // Проходимся по всем доступным комбо
                foreach (var comboConfig in combinableComp.AvailableCombos)
                {
                    if (IsComboAllowedForStance(comboConfig, requestedComboAttackEntity) == false)
                    {
                        continue;
                    }

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
                        _cachedPriority = GetComboPriority(comboConfig, requestedComboAttackEntity);
                        _cachedLastPress = GetComboLastPress(comboConfig, requestedComboAttackEntity);
                    }
                    else if (maxConditions == _cachedConditions)
                    {
                        var currentPriority = GetComboPriority(comboConfig, requestedComboAttackEntity);
                        if (currentPriority > _cachedPriority)
                        {
                            _cachedConditions = maxConditions;
                            _cachedActualConfig = comboConfig;
                            _cachedPriority = currentPriority;
                            _cachedLastPress = GetComboLastPress(comboConfig, requestedComboAttackEntity);
                        }
                        else if (currentPriority == _cachedPriority)
                        {
                            var currentLastPress = GetComboLastPress(comboConfig, requestedComboAttackEntity);
                            if (currentLastPress > _cachedLastPress)
                            {
                                _cachedConditions = maxConditions;
                                _cachedActualConfig = comboConfig;
                                _cachedPriority = currentPriority;
                                _cachedLastPress = currentLastPress;
                            }
                        }
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

        private bool IsComboAllowedForStance(ComboConfig_v2 comboConfig, int entity)
        {
            if (comboConfig == null)
            {
                return false;
            }

            switch (comboConfig.StanceCondition)
            {
                case ComboStanceCondition.Any:
                    return true;
                case ComboStanceCondition.GroundedOnly:
                    return TryGetIsGrounded(entity, out var isGrounded) && isGrounded;
                case ComboStanceCondition.AirborneOnly:
                    return TryGetIsGrounded(entity, out isGrounded) && isGrounded == false;
                default:
                    return true;
            }
        }

        private bool TryGetIsGrounded(int entity, out bool isGrounded)
        {
            isGrounded = false;

            if (_characterControllerPool.Value.Has(entity) == false)
            {
                return false;
            }

            ref var characterController = ref _characterControllerPool.Value.Get(entity);
            if (characterController.Value == null || characterController.Value.Motor == null)
            {
                return false;
            }

            isGrounded = characterController.Value.Motor.GroundingStatus.IsStableOnGround;
            return true;
        }
        
        private bool IsPassedCondition(PlayerAction playerAction, int entity, float leadTime)
        {
            var lastPress = GetLastPress(playerAction, entity);
            return _cachedTime - lastPress <= Mathf.Max(0f, leadTime);
        }

        private float GetLastPress(PlayerAction playerAction, int entity)
        {
            switch (playerAction)
            {
                case PlayerAction.MeleeAttack:
                    return _inputtedMeleeAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.RangeAttack:
                    return _inputtedRangeAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.DoubleMeleeAttack:
                    return _inputtedDoubleMeleeAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.LongMeleeAttack:
                    return _inputtedLongMeleeAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.DoubleRangeAttack:
                    return _inputtedDoubleRangeAttackPool.Value.Get(entity).LastPress;
                case PlayerAction.LongRangeAttack:
                    return _inputtedLongRangeAttackPool.Value.Get(entity).LastPress;
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

        private int GetComboPriority(ComboConfig_v2 comboConfig, int entity)
        {
            var priority = int.MinValue;
            
            for (int i = 0; i < comboConfig.PlayerActions.Count; i++)
            {
                var actionPriority = GetActionPriority(comboConfig.PlayerActions[i]);
                if (actionPriority > priority)
                {
                    priority = actionPriority;
                }
            }

            return priority;
        }

        private float GetComboLastPress(ComboConfig_v2 comboConfig, int entity)
        {
            var lastPress = Single.MinValue;
            
            for (int i = 0; i < comboConfig.PlayerActions.Count; i++)
            {
                var actionLastPress = GetLastPress(comboConfig.PlayerActions[i], entity);
                if (actionLastPress > lastPress)
                {
                    lastPress = actionLastPress;
                }
            }

            return lastPress;
        }

        private int GetActionPriority(PlayerAction playerAction)
        {
            switch (playerAction)
            {
                case PlayerAction.DoubleMeleeAttack:
                    return 4;
                case PlayerAction.MeleeAttack:
                    return 3;
                case PlayerAction.RangeAttack:
                    return 2;
                case PlayerAction.LongMeleeAttack:
                    return 1;
                case PlayerAction.DoubleRangeAttack:
                    return 2;
                case PlayerAction.LongRangeAttack:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
