using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputRangeAttackSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputRangeAttackEvent>> _inputRangeAttackFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputRangeAttackEvent> _inputRangeAttackPool = default;

        private bool _needToCreateRangeAttackEvent;

        public void Init(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.RangeAttack.started += OnInputtedRangeAttack;
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var rangeAttackEventEntity in _inputRangeAttackFilter.Value)
            {
                _inputRangeAttackPool.Value.Del(rangeAttackEventEntity);
            }

            if (_needToCreateRangeAttackEvent == false)
            {
                return;
            }

            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                _inputRangeAttackPool.Value.Add(inputControlsEntity);
            }
                
            _needToCreateRangeAttackEvent = false;
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
                inputControlsComp.Value.GeneralMap.RangeAttack.started -= OnInputtedRangeAttack;
            }
        }

        private void OnInputtedRangeAttack(InputAction.CallbackContext callbackContext)
        {
            _needToCreateRangeAttackEvent = true;
        }
    }
}
