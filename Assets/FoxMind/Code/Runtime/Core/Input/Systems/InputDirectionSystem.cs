using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InputDirectionSystem : BaseEcsVisitable, IEcsPreInitSystem, IEcsRunSystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        readonly EcsFilterInject<Inc<BaseInputControlsComp, InputDirectionComp>> _inputDirectionControlsFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;

        public void PreInit(IEcsSystems systems)
        {
            foreach (var inputEntity in _baseInputControlsFilter.Value)
            {
                _inputDirectionPool.Value.Add(inputEntity);
            }
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var inputEntity in _inputDirectionControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputEntity);
                ref var inputDirectionComp = ref _inputDirectionPool.Value.Get(inputEntity);
                inputDirectionComp.Direction = inputControlsComp.Value.GeneralMap.MoveDirection.ReadValue<Vector2>();
            }
        }
    }
}