using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Cancels current attack/recovery on modifier inputs (dash/jump). Combo continuation is not allowed here.
    /// </summary>
    public class CancelAttackOnModifierInputSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;
        private readonly EcsFilterInject<Inc<InAttackRecoveryComp>> _inAttackRecoveryFilter = default;
        private readonly EcsFilterInject<Inc<InputDashEvent>> _inputDashFilter = default;
        private readonly EcsFilterInject<Inc<InputJumpEvent>> _inputJumpFilter = default;
        private readonly EcsFilterInject<Inc<InputBlockEvent>> _inputBlockFilter = default;

        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;
        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<AttackMovementUnlockRequest> _attackMovementUnlockRequestPool = default;
        private readonly EcsPoolInject<RegisterMotionAnimationRequest> _registerMotionAnimationRequestPool = default;
        private readonly EcsPoolInject<InputtedMeleeAttackComp> _inputtedMeleePool = default;
        private readonly EcsPoolInject<InputtedRangeAttackComp> _inputtedRangePool = default;
        private readonly EcsPoolInject<InputtedDoubleMeleeAttackComp> _inputtedDoubleMeleePool = default;
        private readonly EcsPoolInject<InputtedLongMeleeAttackComp> _inputtedLongMeleePool = default;
        private readonly EcsPoolInject<InputtedDoubleRangeAttackComp> _inputtedDoubleRangePool = default;
        private readonly EcsPoolInject<InputtedLongRangeAttackComp> _inputtedLongRangePool = default;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() == 0
                && _inputJumpFilter.Value.GetEntitiesCount() == 0
                && _inputBlockFilter.Value.GetEntitiesCount() == 0)
            {
                return;
            }

            foreach (var inAttackEntity in _inAttackFilter.Value)
            {
                CancelAttack(inAttackEntity);
            }

            foreach (var inAttackRecoveryEntity in _inAttackRecoveryFilter.Value)
            {
                CancelAttack(inAttackRecoveryEntity);
            }
        }

        private void CancelAttack(int entity)
        {
            if (_inAttackPool.Value.Has(entity))
            {
                _inAttackPool.Value.Del(entity);
            }
            
            if (_inAttackRecoveryPool.Value.Has(entity))
            {
                _inAttackRecoveryPool.Value.Del(entity);
            }

            if (_inComboPool.Value.Has(entity))
            {
                _inComboPool.Value.Del(entity);
            }
            
            ClearBufferedAttacks(entity);
            
            if (_attackMovementUnlockRequestPool.Value.Has(entity) == false)
            {
                _attackMovementUnlockRequestPool.Value.Add(entity);
            }

            _registerMotionAnimationRequestPool.Value.Add(entity);
        }

        private void ClearBufferedAttacks(int entity)
        {
            if (_inputtedMeleePool.Value.Has(entity))
            {
                _inputtedMeleePool.Value.Get(entity).LastPress = float.MinValue;
            }
            if (_inputtedRangePool.Value.Has(entity))
            {
                _inputtedRangePool.Value.Get(entity).LastPress = float.MinValue;
            }
            if (_inputtedDoubleMeleePool.Value.Has(entity))
            {
                _inputtedDoubleMeleePool.Value.Get(entity).LastPress = float.MinValue;
            }
            if (_inputtedLongMeleePool.Value.Has(entity))
            {
                _inputtedLongMeleePool.Value.Get(entity).LastPress = float.MinValue;
            }
            if (_inputtedDoubleRangePool.Value.Has(entity))
            {
                _inputtedDoubleRangePool.Value.Get(entity).LastPress = float.MinValue;
            }
            if (_inputtedLongRangePool.Value.Has(entity))
            {
                _inputtedLongRangePool.Value.Get(entity).LastPress = float.MinValue;
            }
        }
    }
}
