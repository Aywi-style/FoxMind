using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerDashSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlayerInputComp>> _inputEntityFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp>> _controlledTransformFilter = default;
        
        readonly EcsPoolInject<PlayerInputComp> _playerInputPool = default;
        readonly EcsPoolInject<TransformComp> _transformPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var inputEntity in _inputEntityFilter.Value)
            {
                ref var input = ref _playerInputPool.Value.Get(inputEntity);
                
                if (input.Jump)
                {
                    DoDash();
                }
            }
        }

        private void DoDash()
        {
            foreach (var transformEntity in _controlledTransformFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(transformEntity);

                transform.Value.position += transform.Value.forward * 5;
            }
        }
    }
}