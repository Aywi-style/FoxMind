using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public sealed class PlayerInputSystem : BaseEcsVisitable, IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<PlayerInputComp>> _inputEntityFilter = default;
        readonly EcsPoolInject<PlayerInputComp> _playerInputPool = default;
        
        private BaseControls _baseControls;

        private bool _needToCreateJumpEntity;
        private bool _needToCreateAttackEntity;

        public void PreInit(IEcsSystems systems)
        {
            _baseControls = new BaseControls();
            var world = systems.GetWorld();
            var inputEntity = world.NewEntity();
            _playerInputPool.Value.Add(inputEntity);
        }

        public void Init(IEcsSystems systems)
        {
            _baseControls.Enable();
            _baseControls.GeneralMap.Jump.performed += OnPlayerJump;
            _baseControls.GeneralMap.Attack.started += OnPlayerAttack;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var inputEntity in _inputEntityFilter.Value)
            {
                ref var input = ref _playerInputPool.Value.Get(inputEntity);
                input.Direction = _baseControls.GeneralMap.MoveDirection.ReadValue<Vector2>();
                input.Jump = _needToCreateJumpEntity;
                input.Attack = _needToCreateAttackEntity;
            }

            _needToCreateJumpEntity = false;
            _needToCreateAttackEntity = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            _baseControls.GeneralMap.Jump.performed -= OnPlayerJump;
            _baseControls.Disable();
        }

        private void OnPlayerJump(InputAction.CallbackContext ctx)
        {
            _needToCreateJumpEntity = true;
        }

        private void OnPlayerAttack(InputAction.CallbackContext callbackContext)
        {
            _needToCreateAttackEntity = true;
        }
    }
}