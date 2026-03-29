using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class CatchInputAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private const float c_defaultEarlyCancelStart = 0.15f;
        private const float c_defaultEarlyCancelEnd = 0.45f;
        private const float c_bufferTtl = 0.35f;
        
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InputMeleeAttackEvent>> _inputMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputRangeAttackEvent>> _inputRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputDoubleMeleeAttackEvent>> _inputDoubleMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputLongMeleeAttackEvent>> _inputLongMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputDoubleRangeAttackEvent>> _inputDoubleRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputLongRangeAttackEvent>> _inputLongRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<SelfDefineWhatComboNeedToDoRequest>> _playerControlledFilter = default;

        private readonly EcsPoolInject<SelfDefineWhatComboNeedToDoRequest> _comboAttackRequestPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;
        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<InputtedMeleeAttackComp> _inputtedMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedRangeAttackComp> _inputtedRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleMeleeAttackComp> _inputtedDoubleMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongMeleeAttackComp> _inputtedLongMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleRangeAttackComp> _inputtedDoubleRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongRangeAttackComp> _inputtedLongRangeAttackPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var playerControlledEntity in _playerControlledFilter.Value)
            {
                if (HasBufferedAttackInput(playerControlledEntity) == false)
                {
                    continue;
                }
                
                if (CanStartAttack(playerControlledEntity) == false)
                {
                    continue;
                }
                
                _comboAttackRequestPool.Value.Add(playerControlledEntity);
            }
        }

        private bool HasBufferedAttackInput(int entity)
        {
            if (_inputMeleeAttackEventFilter.Value.GetEntitiesCount() > 0
                || _inputRangeAttackEventFilter.Value.GetEntitiesCount() > 0
                || _inputDoubleMeleeAttackEventFilter.Value.GetEntitiesCount() > 0
                || _inputLongMeleeAttackEventFilter.Value.GetEntitiesCount() > 0
                || _inputDoubleRangeAttackEventFilter.Value.GetEntitiesCount() > 0
                || _inputLongRangeAttackEventFilter.Value.GetEntitiesCount() > 0)
            {
                return true;
            }
            
            var time = Time.time;
            if (_inputtedMeleeAttackPool.Value.Has(entity) && time - _inputtedMeleeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }
            if (_inputtedRangeAttackPool.Value.Has(entity) && time - _inputtedRangeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }
            if (_inputtedDoubleMeleeAttackPool.Value.Has(entity) && time - _inputtedDoubleMeleeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }
            if (_inputtedLongMeleeAttackPool.Value.Has(entity) && time - _inputtedLongMeleeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }
            if (_inputtedDoubleRangeAttackPool.Value.Has(entity) && time - _inputtedDoubleRangeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }
            if (_inputtedLongRangeAttackPool.Value.Has(entity) && time - _inputtedLongRangeAttackPool.Value.Get(entity).LastPress <= c_bufferTtl)
            {
                return true;
            }

            return false;
        }

        private bool CanStartAttack(int entity)
        {
            if (_inAttackPool.Value.Has(entity))
            {
                ref var inAttack = ref _inAttackPool.Value.Get(entity);

                if (IsInEarlyCancelWindow(inAttack) == false && IsInComboWindow(entity) == false)
                {
                    return false;
                }
            }

            if (_inAttackRecoveryPool.Value.Has(entity))
            {
                if (IsInComboWindow(entity) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsInComboWindow(int entity)
        {
            if (_inComboPool.Value.Has(entity) == false)
            {
                return false;
            }

            ref var inCombo = ref _inComboPool.Value.Get(entity);
            var time = Time.time;
            return time >= inCombo.NextComboWindowStart && time <= inCombo.NextComboWindowEnd;
        }

        private bool IsInEarlyCancelWindow(InAttackComp inAttack)
        {
            if (inAttack.AttackConfig == null || inAttack.AttackConfig.AttackAnimation == null)
            {
                return false;
            }

            var animLength = inAttack.AttackConfig.AttackAnimation.length;
            if (animLength <= 0)
            {
                return false;
            }
            
            var normalizedTime = (Time.time - inAttack.Start) / animLength;
            var window = GetWindowOrDefault(inAttack.AttackConfig.EarlyCancelWindow, c_defaultEarlyCancelStart, c_defaultEarlyCancelEnd);
            
            return normalizedTime >= window.x && normalizedTime <= window.y;
        }

        private Vector2 GetWindowOrDefault(Vector2 window, float defaultStart, float defaultEnd)
        {
            if (Mathf.Approximately(window.x, 0f) && Mathf.Approximately(window.y, 0f))
            {
                return new Vector2(defaultStart, defaultEnd);
            }

            return new Vector2(Mathf.Min(window.x, window.y), Mathf.Max(window.x, window.y));
        }
    }
}
