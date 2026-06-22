using System;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Input.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class FullComboInputHistorySystem : BaseEcsVisitable, IEcsRunSystem
    {
        private const float c_deadZone = 0.1f;
        
        private readonly EcsWorldInject _defaultWorld = default;
        
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp, TargetingComp>> _comboFilter = default;
        
        private readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        private readonly EcsPoolInject<MoveableComp> _mobablePool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);

                HandleMoves(ref inputControlsComp);
                HandleAttacks(ref inputControlsComp);
            }
        }

        private void HandleAttacks(ref BaseInputControlsComp inputControlsComp)
        {
            var attackInput = ComboInputType.MeleeAttack;
            var altAttackInput = ComboInputType.AltMeleeAttack;
            
            if (inputControlsComp.Value.GeneralMap.WeaponMode.IsPressed())
            {
                attackInput = ComboInputType.RangeAttack;
                altAttackInput = ComboInputType.AltRangeAttack;
            }
            
            if (inputControlsComp.Value.GeneralMap.Attack.WasPressedThisFrame())
            {
                inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(attackInput, PressType.Started));
            }

            if (inputControlsComp.Value.GeneralMap.Attack.WasReleasedThisFrame())
            {
                inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(attackInput, PressType.Cancelled));
            }
            
            if (inputControlsComp.Value.GeneralMap.AltAttack.WasPressedThisFrame())
            {
                inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(altAttackInput, PressType.Started));
            }

            if (inputControlsComp.Value.GeneralMap.AltAttack.WasReleasedThisFrame())
            {
                inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(altAttackInput, PressType.Cancelled));
            }
        }

        private void HandleMoves(ref BaseInputControlsComp inputControlsComp)
        {
            foreach (var comboEntity in _comboFilter.Value)
            {
                ref var targetingComp = ref _targetingPool.Value.Get(comboEntity);

                if (targetingComp.HasHardTarget == false && targetingComp.HasSoftTarget == false &&
                    targetingComp.IsManualAiming == false)
                {
                    continue;
                }
                
                ref var moveable = ref _mobablePool.Value.Get(comboEntity);
                ref var transform = ref _transformPool.Value.Get(comboEntity);

                float dotX = Vector3.Dot(transform.Value.right, moveable.NormalizedMoveDirection);
                float dotY = Vector3.Dot(transform.Value.forward, moveable.NormalizedMoveDirection);

                var absX = Math.Abs(dotX);
                var absY = Math.Abs(dotY);
                
                if (absX < c_deadZone && absY < c_deadZone)
                {
                    PerformComboMoveInput(ref inputControlsComp, ComboInputType.None);
                    return;
                }
                
                var handledMove = ComboInputType.None;
                
                if (absX > absY)
                {
                    switch (dotX)
                    {
                        case > c_deadZone:
                            handledMove = ComboInputType.MoveRight;
                            break;
                        case < -c_deadZone:
                            handledMove = ComboInputType.MoveLeft;
                            break;
                    }
                }
                else
                {
                    switch (dotY)
                    {
                        case > c_deadZone:
                            handledMove = ComboInputType.MoveForward;
                            break;
                        case < -c_deadZone:
                            handledMove = ComboInputType.MoveBackward;
                            break;
                    }
                }
                
                PerformComboMoveInput(ref inputControlsComp, handledMove);
            }
        }

        private void PerformComboMoveInput(ref BaseInputControlsComp inputControlsComp, ComboInputType comboMoveInput)
        {
            if (comboMoveInput == ComboInputType.None)
            {
                if (inputControlsComp.CurrentComboMoveInput != ComboInputType.None)
                {
                    inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(inputControlsComp.CurrentComboMoveInput, PressType.Cancelled));
                    inputControlsComp.CurrentComboMoveInput = ComboInputType.None;
                }
                    
                return;
            }
            
            if (inputControlsComp.CurrentComboMoveInput != comboMoveInput)
            {
                if (inputControlsComp.CurrentComboMoveInput != ComboInputType.None)
                {
                    inputControlsComp.BufferComboInputHistory.Push(
                        ComboInputData.Create(inputControlsComp.CurrentComboMoveInput, PressType.Cancelled));
                }
                
                inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(comboMoveInput, PressType.Started));
            }
                
            inputControlsComp.BufferComboInputHistory.Push(ComboInputData.Create(comboMoveInput, PressType.Performed));
            inputControlsComp.CurrentComboMoveInput = comboMoveInput;
        }
    }
}
