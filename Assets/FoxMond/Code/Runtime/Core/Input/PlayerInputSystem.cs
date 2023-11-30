using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fearness.Code.Runtime.Core.Input
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputSystem : SystemBase
    {
        private BaseControls _baseControls;

        protected override void OnCreate()
        {
            base.OnCreate();

            _baseControls = new BaseControls();

            var inputEntity = EntityManager.CreateEntity();
            EntityManager.AddComponent<CPlayerInput>(inputEntity);
            
            EntityManager.AddComponent<CJumpSignal>(inputEntity);
            EntityManager.SetComponentEnabled<CJumpSignal>(inputEntity, false);
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            
            _baseControls.Enable();
            _baseControls.GeneralMap.Jump.performed += OnPlayerJump;
        }

        protected override void OnUpdate()
        {
            SystemAPI.SetSingleton(new CPlayerInput
            {
                Value = _baseControls.GeneralMap.MoveDirection.ReadValue<Vector2>()
            } );
        }

        protected override void OnStopRunning()
        {
            _baseControls.GeneralMap.Jump.performed -= OnPlayerJump;
            _baseControls.Disable();

            base.OnStopRunning();
        }

        private void OnPlayerJump(InputAction.CallbackContext ctx)
        {
            SystemAPI.SetComponentEnabled<CJumpSignal>(SystemAPI.GetSingletonEntity<CPlayerInput>(), true);
        }
    }
}