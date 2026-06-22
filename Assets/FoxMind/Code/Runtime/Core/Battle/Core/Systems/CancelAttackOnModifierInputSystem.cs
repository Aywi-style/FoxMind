using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    public class CancelAttackOnModifierInputSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;
        private readonly EcsFilterInject<Inc<InAttackRecoveryComp>> _inAttackRecoveryFilter = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;
        private readonly EcsPoolInject<InAttackMovementComp> _inAttackMovementPool = default;
        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<AttackMovementUnlockRequest> _attackMovementUnlockRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);
                if (inputControlsComp.Value.GeneralMap.Jump.WasPressedThisFrame() == false &&
                    inputControlsComp.Value.GeneralMap.Defence.WasPressedThisFrame() == false)
                {
                    return;
                }
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
            
            if (_inAttackMovementPool.Value.Has(entity))
            {
                _inAttackMovementPool.Value.Del(entity);
            }
            
            if (_attackMovementUnlockRequestPool.Value.Has(entity) == false)
            {
                _attackMovementUnlockRequestPool.Value.Add(entity);
            }
        }
    }
}
