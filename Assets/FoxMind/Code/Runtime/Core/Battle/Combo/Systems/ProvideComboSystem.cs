using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Features;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Input.Structs;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.ProjectScope;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class ProvideComboSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;
        // todo Добавить Exc с состояниями в фильтр ниже
        private readonly EcsFilterInject<Inc<PlayerControlledComp, CombinableComp>> _combinableCharacterFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combosPool = default;
        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<ProvideAttackRequest> _targetProvideAttackRequestPool = default;
        
        private float _cachedTime;
        private ComboConfig _matchedCombo;
        private float _debugActivatorTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            _matchedCombo = null;

            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);

                int activatorInputIndex = -1;
                ComboInputData activatorInputData = default;
                
                for (int i = 0; i < inputControlsComp.BufferComboInputHistory.Count; i++)
                {
                    if (inputControlsComp.BufferComboInputHistory[i].IsComboActivator() == false) // кнопка либо не является активатором
                    {
                        // Debug.Log("Не активатор");
                        
                        continue;
                    }
                    
                    if (inputControlsComp.BufferComboInputHistory[i].Time > _cachedTime - R.TimeForReedAttackInput) // кнопка рано нажата
                    {
                        //Debug.Log($"Рано {inputControlsComp.BufferComboInputHistory[i].InputData.Type}");
                        
                        continue;
                    }
                    
                    if (inputControlsComp.BufferComboInputHistory[i].Time < _cachedTime - R.LastTimeForReedAttackInput) // кнопка нажата поздно
                    {
                        //Debug.Log($"Поздно {inputControlsComp.BufferComboInputHistory[i].InputData.Type}");
                        
                        break;
                    }
                    
                    activatorInputData = inputControlsComp.BufferComboInputHistory[i];
                    activatorInputIndex = i;
                    _debugActivatorTime = activatorInputData.Time;
                }

                if (activatorInputIndex == -1) // не нашли ни одного активатора комбо
                {
                    continue;
                }

                // Определяем с каким комбо можем сметчиться
                foreach (var combinableEntity in _combinableCharacterFilter.Value)
                {
                    ref var combinableComp = ref _combosPool.Value.Get(combinableEntity);

                    foreach (var availableCombo in combinableComp.AvailableCombos)
                    {
                        // Присутствует-ли нажатый активатор в комбо?
                        if (availableCombo.ComboActivators.Contains(activatorInputData.InputData) == false)
                        {
                            continue;
                        }

                        // Если активаторов комбо больше одного, то мэтчим оставшиеся активаторы
                        if (availableCombo.ComboActivators.Count > 1)
                        {
                            // Проверяем оставшиеся активаторы на мэтч
                            bool activatorsMatched = true;
                            
                            foreach (var comboActivator in availableCombo.ComboActivators)
                            {
                                bool currentActivatorMatched = false;
                                
                                for (int i = activatorInputIndex; i >= 0; i--)
                                {
                                    if (inputControlsComp.BufferComboInputHistory[i].IsComboActivator() == false)
                                    {
                                        continue;
                                    }

                                    currentActivatorMatched = comboActivator.Equals(inputControlsComp.BufferComboInputHistory[i].InputData);

                                    if (currentActivatorMatched)
                                    {
                                        break;
                                    }
                                }

                                if (currentActivatorMatched == false)
                                {
                                    activatorsMatched = false;
                                    break;
                                }
                            }

                            if (activatorsMatched == false)
                            {
                                continue;
                            }
                        }

                        // Проверка на правильную последовательность движений в комбо
                        if (availableCombo.MovementSetup.Count > 0)
                        {
                            int moveInputIndexForMatch = activatorInputIndex + 1;
                            int movementMatchCount = 0;
                            
                            // Проверка идёт в обратном порядке, чтобы в конфигах можно было человекоподобно устанавливать секвенцию движений

                            for (int i = availableCombo.MovementSetup.Count - 1; i >= 0; i--)
                            {
                                for (int j = moveInputIndexForMatch; j < inputControlsComp.BufferComboInputHistory.Count; j++)
                                {
                                    if (inputControlsComp.BufferComboInputHistory[j].IsMove() == false)
                                    {
                                        continue;
                                    }

                                    if (inputControlsComp.BufferComboInputHistory[j].Time < activatorInputData.Time - R.LastTimeForReedMoveInput)
                                    {
                                        // Значит всё комбо уже не валидно
                                        break;
                                    }

                                    if (availableCombo.MovementSetup[i].Equals(inputControlsComp.BufferComboInputHistory[j].InputData))
                                    {
                                        moveInputIndexForMatch = j + 1;
                                        movementMatchCount++;
                                        
                                        Debug.Log($"Мэтч [{j}] {inputControlsComp.BufferComboInputHistory[j].InputData.Type}|{inputControlsComp.BufferComboInputHistory[j].InputData.PressType}|{inputControlsComp.BufferComboInputHistory[j].Time}");
                                        break;
                                    }
                                }

                                // Выход из мэтча с конфигом, если хотя бы один мэтч по движению не произошёл
                                if (movementMatchCount + i != availableCombo.MovementSetup.Count)
                                {
                                    break;
                                }
                            }

                            if (movementMatchCount != availableCombo.MovementSetup.Count)
                            {
                                continue;
                            }
                        }

                        // Записываем в кэш наиболее приоритетное сметченное комбо
                        if (_matchedCombo == null)
                        {
                            _matchedCombo = availableCombo;
                        }
                        else
                        {
                            if (_matchedCombo.GetPriority() < availableCombo.GetPriority())
                            {
                                _matchedCombo = availableCombo;
                            }
                        }
                    }
                    
                    if (_matchedCombo != null)
                    {
                        Debug.Log($"Активатор. Время {_debugActivatorTime} | Запас для мувмента: {_debugActivatorTime - R.LastTimeForReedMoveInput}");
                        Debug.Log($"Matched: {_matchedCombo.name}");
                        
                        combinableComp.CurrentCombo = _matchedCombo;
                        
                        ref var targetProvideAttackRequest = ref _targetProvideAttackRequestPool.Value.Add(_world.Value.NewEntity());
                        targetProvideAttackRequest.AttackConfig = combinableComp.CurrentCombo.AttackConfig;
                        targetProvideAttackRequest.PackedEntity = _world.Value.PackEntity(combinableEntity);
                        
                        inputControlsComp.BufferComboInputHistory.Clear();
                    }
                }
            }
        }
    }
}
