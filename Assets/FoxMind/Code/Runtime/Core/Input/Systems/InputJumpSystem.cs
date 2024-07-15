using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputJumpSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputJumpEvent>> _inputJumpFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputJumpEvent> _inputJumpPool = default;

        private bool _needToCreateJumpEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Jump.started += OnInputtedJump;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var attackEventEntity in _inputJumpFilter.Value)
            {
                _inputJumpPool.Value.Del(attackEventEntity);
            }
            
            if (_needToCreateJumpEvent == false)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputJumpPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreateJumpEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Jump.started -= OnInputtedJump;
            }
        }

        private void OnInputtedJump(InputAction.CallbackContext callbackContext)
        {
            _needToCreateJumpEvent = true;
        }
    }
}