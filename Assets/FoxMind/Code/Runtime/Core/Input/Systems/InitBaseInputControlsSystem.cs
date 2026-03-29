using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InitBaseInputControlsSystem : BaseEcsVisitable, IEcsPreInitSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        readonly EcsPoolInject<MeleeInputStateComp> _meleeInputStatePool = default;
        readonly EcsPoolInject<RangeInputStateComp> _rangeInputStatePool = default;
        
        private BaseControls _baseControls;

        public void PreInit(IEcsSystems systems)
        {
            var baseInputControlsEntity = _defaultWorld.Value.NewEntity(); 
            ref var baseInputControlsComp = ref _baseInputControlsPool.Value.Add(baseInputControlsEntity);
            baseInputControlsComp.Value = new BaseControls();
            baseInputControlsComp.Value.Enable();

            if (_meleeInputStatePool.Value.Has(baseInputControlsEntity) == false)
            {
                _meleeInputStatePool.Value.Add(baseInputControlsEntity);
            }
            
            if (_rangeInputStatePool.Value.Has(baseInputControlsEntity) == false)
            {
                _rangeInputStatePool.Value.Add(baseInputControlsEntity);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            foreach (var baseInputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var baseInputControlsComp = ref _baseInputControlsPool.Value.Get(baseInputControlsEntity);
                baseInputControlsComp.Value.Disable();
            }
        }
    }
}
