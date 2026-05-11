using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
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
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InputMeleeAttackEvent>> _inputMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputRangeAttackEvent>> _inputRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputDoubleMeleeAttackEvent>> _inputDoubleMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputLongMeleeAttackEvent>> _inputLongMeleeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputDoubleRangeAttackEvent>> _inputDoubleRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<InputLongRangeAttackEvent>> _inputLongRangeAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<SelfDefineWhatComboNeedToDoRequest>> _playerControlledFilter = default;

        private readonly EcsPoolInject<SelfDefineWhatComboNeedToDoRequest> _comboAttackRequestPool = default;
        private readonly EcsPoolInject<CombinableComp> _combinablePool = default;
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

            if (_combinablePool.Value.Has(entity) == false)
            {
                return false;
            }

            ref var combinable = ref _combinablePool.Value.Get(entity);
            if (combinable.AvailableCombos == null)
            {
                return false;
            }

            var time = Time.time;

            foreach (var comboConfig in combinable.AvailableCombos)
            {
                if (HasFreshAttackAction(entity, comboConfig, time))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CanStartAttack(int entity)
        {
            if (_inAttackPool.Value.Has(entity))
            {
                if (IsInComboWindow(entity) == false)
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
            return time >= inCombo.NextComboWindowStart - GetMaxNextComboLeadTime(ref inCombo)
                   && time <= inCombo.NextComboWindowEnd;
        }

        private float GetMaxNextComboLeadTime(ref InComboComp inCombo)
        {
            if (inCombo.ComboConfig == null || inCombo.ComboConfig.NextCombos == null)
            {
                return 0f;
            }

            var maxLeadTime = 0f;
            foreach (var comboConfig in inCombo.ComboConfig.NextCombos)
            {
                if (comboConfig != null && comboConfig.LeadTime > maxLeadTime)
                {
                    maxLeadTime = comboConfig.LeadTime;
                }
            }

            return maxLeadTime;
        }

        private bool HasFreshAttackAction(int entity, ComboConfig_v2 comboConfig, float time)
        {
            if (comboConfig == null || comboConfig.PlayerActions == null)
            {
                return false;
            }

            var leadTime = Mathf.Max(0f, comboConfig.LeadTime);
            foreach (var playerAction in comboConfig.PlayerActions)
            {
                if (IsAttackAction(playerAction) == false)
                {
                    continue;
                }

                if (time - GetLastPress(playerAction, entity) <= leadTime)
                {
                    return true;
                }
            }

            return false;
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
                default:
                    return float.MinValue;
            }
        }

        private bool IsAttackAction(PlayerAction playerAction)
        {
            switch (playerAction)
            {
                case PlayerAction.MeleeAttack:
                case PlayerAction.RangeAttack:
                case PlayerAction.DoubleMeleeAttack:
                case PlayerAction.LongMeleeAttack:
                case PlayerAction.DoubleRangeAttack:
                case PlayerAction.LongRangeAttack:
                    return true;
                default:
                    return false;
            }
        }
    }
}
