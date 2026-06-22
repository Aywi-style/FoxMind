using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Input.Enums;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Systems
{
    /// <summary>
    /// Обрабатывает режим ручного прицеливания при удержании кнопки таргетинга.
    /// Для геймпада читает правый стик, для мыши считает направление от игрока к курсору на плоскости движения.
    /// </summary>
    public class TargetingManualAimSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputTargetLockPerformedComp>> _targetLockPerformedFilter = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, TargetingComp, TargetingStateComp>> _targetingFilter = default;
        private readonly EcsFilterInject<Inc<CameraComp, TransformComp>> _cameraFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        private readonly EcsPoolInject<TargetingStateComp> _targetingStatePool = default;
        private readonly EcsPoolInject<CameraComp> _cameraPool = default;

        public void Run(IEcsSystems systems)
        {
            var isTargetHeld = _targetLockPerformedFilter.Value.GetEntitiesCount() > 0;
            var camera = GetCamera();
            var activeControlType = GetActiveControlType();

            foreach (var targetingEntity in _targetingFilter.Value)
            {
                ref var targeting = ref _targetingPool.Value.Get(targetingEntity);
                ref var state = ref _targetingStatePool.Value.Get(targetingEntity);

                if (isTargetHeld == false || state.IsPressed == false || Time.time - state.PressStartTime < targeting.HoldThreshold)
                {
                    targeting.IsManualAiming = false;
                    targeting.ManualAimDirection = Vector3.zero;
                    continue;
                }

                ref var transform = ref _transformPool.Value.Get(targetingEntity);
                var aimDirection = GetAimDirection(activeControlType, camera, transform.Value.position, targeting.RightStickDeadZone);

                aimDirection.y = 0f;
                if (aimDirection.sqrMagnitude <= float.Epsilon)
                {
                    targeting.IsManualAiming = false;
                    targeting.ManualAimDirection = Vector3.zero;
                    continue;
                }

                targeting.IsManualAiming = true;
                targeting.ManualAimDirection = aimDirection.normalized;
            }
        }

        private InputControlType GetActiveControlType()
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                return _baseInputControlsPool.Value.Get(inputControlsEntity).ActiveControlType;
            }

            return InputControlType.Unknown;
        }

        private Vector3 GetAimDirection(InputControlType activeControlType, UnityEngine.Camera camera, Vector3 origin, float rightStickDeadZone)
        {
            switch (activeControlType)
            {
                case InputControlType.Gamepad:
                    var gamepadDirection = GetGamepadAimDirection(camera);
                    return gamepadDirection.sqrMagnitude > rightStickDeadZone * rightStickDeadZone
                        ? gamepadDirection
                        : Vector3.zero;
                
                case InputControlType.KeyboardMouse:
                case InputControlType.Unknown:
                default:
                    return GetMouseAimDirection(camera, origin);
            }
        }

        private UnityEngine.Camera GetCamera()
        {
            foreach (var cameraEntity in _cameraFilter.Value)
            {
                ref var camera = ref _cameraPool.Value.Get(cameraEntity);
                if (camera.Camera != null)
                {
                    return camera.Camera;
                }
            }

            return null;
        }

        private Vector3 GetGamepadAimDirection(UnityEngine.Camera camera)
        {
            if (Gamepad.current == null)
            {
                return Vector3.zero;
            }

            var stick = Gamepad.current.rightStick.ReadValue();
            if (stick.sqrMagnitude <= float.Epsilon)
            {
                return Vector3.zero;
            }

            if (camera == null)
            {
                return new Vector3(stick.x, 0f, stick.y);
            }

            var cameraForward = camera.transform.forward;
            var cameraRight = camera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            return cameraRight * stick.x + cameraForward * stick.y;
        }

        private Vector3 GetMouseAimDirection(UnityEngine.Camera camera, Vector3 origin)
        {
            if (camera == null || Mouse.current == null)
            {
                return Vector3.zero;
            }

            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var plane = new Plane(Vector3.up, origin);
            if (plane.Raycast(ray, out var enter) == false)
            {
                return Vector3.zero;
            }

            return ray.GetPoint(enter) - origin;
        }
    }
}
