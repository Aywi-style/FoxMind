using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking.Systems
{
    public class TrackMeleeInputSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp, MeleeInputStateComp>> _inputFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedMeleeAttackComp>> _playerControlledMeleeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedDoubleMeleeAttackComp>> _playerControlledDoubleMeleeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedLongMeleeAttackComp>> _playerControlledLongMeleeAttackFilter = default;
        private readonly EcsPoolInject<MeleeInputStateComp> _statePool = default;
        
        private readonly EcsPoolInject<InputMeleeAttackEvent> _meleeEventPool = default;
        private readonly EcsPoolInject<InputDoubleMeleeAttackEvent> _doubleMeleeEventPool = default;
        private readonly EcsPoolInject<InputLongMeleeAttackEvent> _longMeleeEventPool = default;
        
        private readonly EcsPoolInject<InputtedMeleeAttackComp> _inputtedMeleePool = default;
        private readonly EcsPoolInject<InputtedDoubleMeleeAttackComp> _inputtedDoubleMeleePool = default;
        private readonly EcsPoolInject<InputtedLongMeleeAttackComp> _inputtedLongMeleePool = default;

        public void Run(IEcsSystems systems)
        {
            var time = Time.time;

            foreach (var inputEntity in _inputFilter.Value)
            {
                ref var state = ref _statePool.Value.Get(inputEntity);
                ref var inputControls = ref systems.GetWorld().GetPool<BaseInputControlsComp>().Get(inputEntity);
                
                var action = inputControls.Value.GeneralMap.MeleeAttack;

                if (action.WasPressedThisFrame())
                {
                    state.IsPressed = true;
                    state.PressStartTime = time;
                    state.LongTriggered = false;

                    if (state.PendingSingle && time - state.LastTapTime <= CombatInputTuning.MeleeDoubleTapWindow)
                    {
                        state.PendingSingle = false;
                        state.LastTapTime = float.MinValue;

                        if (_doubleMeleeEventPool.Value.Has(inputEntity) == false)
                        {
                            _doubleMeleeEventPool.Value.Add(inputEntity);
                        }
                        foreach (var playerControlledDoubleMeleeAttackEntity in _playerControlledDoubleMeleeAttackFilter.Value)  
                        {
                            _inputtedDoubleMeleePool.Value.Get(playerControlledDoubleMeleeAttackEntity).LastPress = time;
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
                    && time - state.PressStartTime >= CombatInputTuning.MeleeLongPressThreshold)
                {
                    state.LongTriggered = true;
                    state.PendingSingle = false;
                    state.LastTapTime = float.MinValue;

                    if (_longMeleeEventPool.Value.Has(inputEntity) == false)
                    {
                        _longMeleeEventPool.Value.Add(inputEntity);
                    }
                    
                    foreach (var playerControlledLongMeleeAttackEntity in _playerControlledLongMeleeAttackFilter.Value)  
                    {
                        _inputtedLongMeleePool.Value.Get(playerControlledLongMeleeAttackEntity).LastPress = time;
                    }
                }

                if (action.WasReleasedThisFrame())
                {
                    state.IsPressed = false;
                }

                if (state.PendingSingle && state.IsPressed == false
                    && time - state.PendingSingleTime >= CombatInputTuning.MeleeDoubleTapWindow)
                {
                    state.PendingSingle = false;

                    if (_meleeEventPool.Value.Has(inputEntity) == false)
                    {
                        _meleeEventPool.Value.Add(inputEntity);
                    }

                    foreach (var playerControlledMeleeAttackEntity in _playerControlledMeleeAttackFilter.Value)  
                    {
                        _inputtedMeleePool.Value.Get(playerControlledMeleeAttackEntity).LastPress = time;
                    }
                }
            }
        }
    }
}
