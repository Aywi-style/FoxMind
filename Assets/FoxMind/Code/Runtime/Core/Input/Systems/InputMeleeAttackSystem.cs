using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputMeleeAttackSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputMeleeAttackEvent>> _inputMeleeAttackFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputMeleeAttackEvent> _inputMeleeAttackPool = default;

        private bool _needToCreateMeleeAttackEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.MeleeAttack.started += OnInputtedMeleeAttack;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var meleeAttackEventEntity in _inputMeleeAttackFilter.Value)
            {
                _inputMeleeAttackPool.Value.Del(meleeAttackEventEntity);
            }

            if (_needToCreateMeleeAttackEvent == false)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputMeleeAttackPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreateMeleeAttackEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.MeleeAttack.started -= OnInputtedMeleeAttack;
            }
        }

        private void OnInputtedMeleeAttack(InputAction.CallbackContext callbackContext)
        {
            _needToCreateMeleeAttackEvent = true;
        }
    }
}
