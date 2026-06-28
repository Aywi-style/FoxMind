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
    public class CancelAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InAttackComp>> _inAttackFilter = default;
        private readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
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
                ref var inAttackComp = ref _inAttackPool.Value.Get(inAttackEntity);

                bool playerInMove = false;
                foreach (var inputDirectionEntity in _inputDirectionFilter.Value)
                {
                    ref var inputDirectionComp = ref _inputDirectionPool.Value.Get(inputDirectionEntity);
                    
                    if (inputDirectionComp.Direction.x != 0 || inputDirectionComp.Direction.y != 0)
                    {
                        playerInMove = true;
                        break;
                    }
                }

                var continuousEnd = inAttackComp.Start + (inAttackComp.AttackConfig.EndOfContinuousPart * inAttackComp.Duration);

                if ((playerInMove && _cachedTime > continuousEnd) || _isDefencePressed || _isJumpPressed || _cachedTime > inAttackComp.End)
                {
                    _inAttackPool.Value.Del(inAttackEntity);
                }
            }
        }
    }
}
