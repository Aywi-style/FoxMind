using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
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
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;

        public void PreInit(IEcsSystems systems)
        {
            var inputDirectionEntity = _defaultWorld.Value.NewEntity(); 
            _inputDirectionPool.Value.Add(inputDirectionEntity);
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var inputEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputEntity);

                foreach (var inputDirectionEntity in _inputDirectionFilter.Value)
                {
                    ref var inputDirectionComp = ref _inputDirectionPool.Value.Get(inputDirectionEntity);

                    inputDirectionComp.Direction = inputControlsComp.Value.GeneralMap.MoveDirection.ReadValue<Vector2>();
                }
            }
        }
    }
}