using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking.Systems
{
    public class TrackRangeInputSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp, RangeInputStateComp>> _inputFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedRangeAttackComp>> _playerControlledRangeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedDoubleRangeAttackComp>> _playerControlledDoubleRangeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedLongRangeAttackComp>> _playerControlledLongRangeAttackFilter = default;
        
        private readonly EcsPoolInject<RangeInputStateComp> _statePool = default;
        
        private readonly EcsPoolInject<InputRangeAttackEvent> _rangeEventPool = default;
        private readonly EcsPoolInject<InputDoubleRangeAttackEvent> _doubleRangeEventPool = default;
        private readonly EcsPoolInject<InputLongRangeAttackEvent> _longRangeEventPool = default;
        
        private readonly EcsPoolInject<InputtedRangeAttackComp> _inputtedRangePool = default;
        private readonly EcsPoolInject<InputtedDoubleRangeAttackComp> _inputtedDoubleRangePool = default;
        private readonly EcsPoolInject<InputtedLongRangeAttackComp> _inputtedLongRangePool = default;

        public void Run(IEcsSystems systems)
        {
            var time = Time.time;

            foreach (var inputEntity in _inputFilter.Value)
            {
                ref var state = ref _statePool.Value.Get(inputEntity);
                ref var inputControls = ref systems.GetWorld().GetPool<BaseInputControlsComp>().Get(inputEntity);
                
                var action = inputControls.Value.GeneralMap.RangeAttack;

                if (action.WasPressedThisFrame())
                {
                    state.IsPressed = true;
                    state.PressStartTime = time;
                    state.LongTriggered = false;

                    if (state.PendingSingle && time - state.LastTapTime <= CombatInputTuning.RangeDoubleTapWindow)
                    {
                        state.PendingSingle = false;
                        state.LastTapTime = float.MinValue;

                        if (_doubleRangeEventPool.Value.Has(inputEntity) == false)
                        {
                            _doubleRangeEventPool.Value.Add(inputEntity);
                        }
                    
                        foreach (var playerControlledDoubleRangeAttackEntity in _playerControlledDoubleRangeAttackFilter.Value)  
                        {
                            _inputtedDoubleRangePool.Value.Get(playerControlledDoubleRangeAttackEntity).LastPress = time;
                        }

                        // Double tap also counts as a fresh single attack for regular combo chains.
                        if (_rangeEventPool.Value.Has(inputEntity) == false)
                        {
                            _rangeEventPool.Value.Add(inputEntity);
                        }
                        
                        foreach (var playerControlledRangeAttackEntity in _playerControlledRangeAttackFilter.Value)  
                        {
                            _inputtedRangePool.Value.Get(playerControlledRangeAttackEntity).LastPress = time;
                        }
                    }
                    else
                    {
                        state.PendingSingle = true;
                        state.PendingSingleTime = time;
                        state.LastTapTime = time;
                    }
                }

                if (state.IsPressed && state.LongTriggered == false
                    && time - state.PressStartTime >= CombatInputTuning.RangeLongPressThreshold)
                {
                    state.LongTriggered = true;
                    state.PendingSingle = false;
                    state.LastTapTime = float.MinValue;

                    if (_longRangeEventPool.Value.Has(inputEntity) == false)
                    {
                        _longRangeEventPool.Value.Add(inputEntity);
                    }
                    
                    foreach (var playerControlledLongRangeAttackEntity in _playerControlledLongRangeAttackFilter.Value)  
                    {
                        _inputtedLongRangePool.Value.Get(playerControlledLongRangeAttackEntity).LastPress = time;
                    }
                }

                if (action.WasReleasedThisFrame())
                {
                    state.IsPressed = false;
                }

                if (state.PendingSingle && state.IsPressed == false
                    && time - state.PendingSingleTime >= CombatInputTuning.RangeDoubleTapWindow)
                {
                    state.PendingSingle = false;

                    if (_rangeEventPool.Value.Has(inputEntity) == false)
                    {
                        _rangeEventPool.Value.Add(inputEntity);
                    }
                    
                    foreach (var playerControlledMeleeAttackEntity in _playerControlledRangeAttackFilter.Value)  
                    {
                        _inputtedRangePool.Value.Get(playerControlledMeleeAttackEntity).LastPress = time;
                    }
                }
            }
        }
    }
}
