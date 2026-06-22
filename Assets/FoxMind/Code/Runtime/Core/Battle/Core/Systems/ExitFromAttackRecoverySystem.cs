using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Система, которая сбрасывает анимацию атаки, если игрок нажимает клавиши передвижения в окно, позволяющее прервать атаку
    /// </summary>
    public class ExitFromAttackRecoverySystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackRecoveryComp>, Exc<InAttackComp>> _inAttackFilter = default;
        private readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<InAttackRecoveryComp> _inAttackRecoveryPool = default;
        private readonly EcsPoolInject<InAttackMovementComp> _inAttackMovementPool = default;
        private readonly EcsPoolInject<AttackMovementUnlockRequest> _attackMovementUnlockRequestPool = default;
        private readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;

        private float _cachedTime;
        private bool _isDefencePressed;
        private bool _isJumpPressed;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            if (_inAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);

                _isJumpPressed = inputControlsComp.Value.GeneralMap.Jump.WasPressedThisFrame();
                _isDefencePressed = inputControlsComp.Value.GeneralMap.Defence.WasPressedThisFrame();
                break;
            }
            
            foreach (var inAttackEntity in _inAttackFilter.Value)
            {
                ref var inAttackRecoveryComp = ref _inAttackRecoveryPool.Value.Get(inAttackEntity);

                bool playerIsFreeze = true;
                foreach (var inputDirectionEntity in _inputDirectionFilter.Value)
                {
                    ref var inputDirectionComp = ref _inputDirectionPool.Value.Get(inputDirectionEntity);
                    
                    if (inputDirectionComp.Direction.x != 0 || inputDirectionComp.Direction.y != 0)
                    {
                        playerIsFreeze = false;
                        break;
                    }
                }

                bool hasModifierInput = playerIsFreeze == false
                                        || _isDefencePressed
                                        || _isJumpPressed;

                var animLength = inAttackRecoveryComp.End - inAttackRecoveryComp.Start;
                var continuousEnd = inAttackRecoveryComp.Start + (inAttackRecoveryComp.AttackConfig.EndOfContinuousPart * animLength);
                
                if (_cachedTime < inAttackRecoveryComp.End && (hasModifierInput == false || _cachedTime < continuousEnd))
                {
                    continue;
                }
                /*foreach (var attackComponent in inAttackComponent.AttackConfig.AttackEndComponents)
                    {
                        attackComponent.Compose(_world.Value, inAttackEntity);
                    }*/

                _inAttackRecoveryPool.Value.Del(inAttackEntity);
                if (_inAttackMovementPool.Value.Has(inAttackEntity))
                {
                    _inAttackMovementPool.Value.Del(inAttackEntity);
                }

                
                if (_attackMovementUnlockRequestPool.Value.Has(inAttackEntity) == false)
                {
                    _attackMovementUnlockRequestPool.Value.Add(inAttackEntity);
                }
            }
        }

    }
}
