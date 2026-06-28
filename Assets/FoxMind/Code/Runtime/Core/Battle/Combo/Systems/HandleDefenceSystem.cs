using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class HandleDefenceSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;

        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_inputControlsFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);

                if (inputControlsComp.Value.GeneralMap.Defence.WasReleasedThisFrame() == false)
                {
                    return;
                }
                
                // handle it here
                
                
            }
        }
    }
}