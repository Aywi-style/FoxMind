using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerDashSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<InputDashEvent>> _inputDashFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            Debug.Log("Performed 1");
            
            foreach (var transformEntity in _controlledTransformFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(transformEntity);

                transform.Value.position += transform.Value.forward * 5;
                
                Debug.Log("Performed 2");
            }
        }
    }
}