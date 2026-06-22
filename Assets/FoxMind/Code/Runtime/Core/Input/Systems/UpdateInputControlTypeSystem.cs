using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Input.Enums;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    /// <summary>
    /// Отслеживает, с какого типа устройства пришёл последний игровой ввод, и сохраняет это в BaseInputControlsComp.
    /// </summary>
    public class UpdateInputControlTypeSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        private readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;

        private InputControlType _pendingControlType;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControls = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControls.Value.GeneralMap.Get().actionTriggered += OnActionTriggered;
            }
        }

        public void Run(IEcsSystems systems)
        {
            if (_pendingControlType == InputControlType.Unknown)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControls = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControls.ActiveControlType = _pendingControlType;
            }

            _pendingControlType = InputControlType.Unknown;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControls = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControls.Value.GeneralMap.Get().actionTriggered -= OnActionTriggered;
            }
        }

        private void OnActionTriggered(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started == false && callbackContext.performed == false)
            {
                return;
            }

            var device = callbackContext.control?.device;
            if (device is Gamepad)
            {
                _pendingControlType = InputControlType.Gamepad;
                return;
            }

            if (device is Keyboard || device is Mouse)
            {
                _pendingControlType = InputControlType.KeyboardMouse;
            }
        }
    }
}
