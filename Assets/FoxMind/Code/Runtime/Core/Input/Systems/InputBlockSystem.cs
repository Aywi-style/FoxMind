using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputBlockSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputBlockEvent>> _inputBlockFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputBlockEvent> _inputBlockPool = default;

        private bool _needToCreateBlockEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Block.started += OnInputtedBlock;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var blockEventEntity in _inputBlockFilter.Value)
            {
                _inputBlockPool.Value.Del(blockEventEntity);
            }

            if (_needToCreateBlockEvent == false)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputBlockPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreateBlockEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.Block.started -= OnInputtedBlock;
            }
        }

        private void OnInputtedBlock(InputAction.CallbackContext callbackContext)
        {
            _needToCreateBlockEvent = true;
        }
    }
}
