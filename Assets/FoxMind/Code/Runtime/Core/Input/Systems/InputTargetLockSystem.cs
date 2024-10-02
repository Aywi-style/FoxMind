using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputTargetLockSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputTargetLockPerformedComp>> _inputTargetLockPerformedFilter = default;
        readonly EcsFilterInject<Inc<InputTargetLockPressDownEvent>> _inputTargetLockPressDownEventFilter = default;
        readonly EcsFilterInject<Inc<InputTargetLockPressUpEvent>> _inputTargetLockPressUpEventFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputTargetLockPerformedComp> _inputTargetLockPerformedCompPool = default;
        readonly EcsPoolInject<InputTargetLockPressDownEvent> _inputTargetLockPressDownEventPool = default;
        readonly EcsPoolInject<InputTargetLockPressUpEvent> _inputTargetLockPressUpEventPool = default;

        private bool _needToCreatePressUpEvent;
        private bool _needToCreatePressDownEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.TargetLock.started += OnInputtedTargetLockDownPressed;
                inputControlsComp.Value.GeneralMap.TargetLock.canceled += OnInputtedTargetLockUpPressed;
            }
        }

        public void Run(IEcsSystems systems)
        {
            DelAllDownEvents();
            DelAllUpEvents();

            PerformedLogic();
            
            TryToCreateDownEvent();
            TryToCreateUpEvent();
        }

        private void DelAllDownEvents()
        {
            foreach (var attackEventEntity in _inputTargetLockPressDownEventFilter.Value)
            {
                _inputTargetLockPressDownEventPool.Value.Del(attackEventEntity);
            }
        }

        private void DelAllUpEvents()
        {
            foreach (var attackEventEntity in _inputTargetLockPressUpEventFilter.Value)
            {
                _inputTargetLockPressUpEventPool.Value.Del(attackEventEntity);
            }
        }

        private void PerformedLogic()
        {
            if (_needToCreatePressUpEvent)
            {
                foreach (var attackEventEntity in _inputTargetLockPerformedFilter.Value)
                {
                    _inputTargetLockPerformedCompPool.Value.Del(attackEventEntity);
                }
            }

            if (_needToCreatePressDownEvent)
            {
                foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
                {
                    if (_inputTargetLockPerformedCompPool.Value.Has(inputControlsEntity) == false)
                    {
                        _inputTargetLockPerformedCompPool.Value.Add(inputControlsEntity);
                    }
                }
            }
        }
        
        private void TryToCreateDownEvent()
        {
            if (_needToCreatePressDownEvent == false)
            {
                return; 
            }
            
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                if (_inputTargetLockPressDownEventPool.Value.Has(inputControlsEntity) == false)
                {
                    _inputTargetLockPressDownEventPool.Value.Add(inputControlsEntity);
                }
            }
                
            _needToCreatePressDownEvent = false;
        }

        private void TryToCreateUpEvent()
        {
            if (_needToCreatePressUpEvent == false)
            {
                return;
            }
            
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputTargetLockPressUpEventPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreatePressUpEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Jump.started -= OnInputtedTargetLockDownPressed;
                inputControlsComp.Value.GeneralMap.Jump.canceled -= OnInputtedTargetLockUpPressed;
            }
        }

        private void OnInputtedTargetLockDownPressed(InputAction.CallbackContext callbackContext)
        {
            _needToCreatePressDownEvent = true;
        }

        private void OnInputtedTargetLockUpPressed(InputAction.CallbackContext callbackContext)
        {
            _needToCreatePressUpEvent = true;
        }
    }
}