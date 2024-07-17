using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputAttackSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputAttackEvent>> _inputAttackFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputAttackEvent> _inputAttackPool = default;

        private bool _needToCreateAttackEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Attack.started += OnInputtedAttack;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var attackEventEntity in _inputAttackFilter.Value)
            {
                _inputAttackPool.Value.Del(attackEventEntity);
            }

            if (_needToCreateAttackEvent == false)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputAttackPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreateAttackEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Attack.started -= OnInputtedAttack;
            }
        }

        private void OnInputtedAttack(InputAction.CallbackContext callbackContext)
        {
            _needToCreateAttackEvent = true;
        }
    }
}