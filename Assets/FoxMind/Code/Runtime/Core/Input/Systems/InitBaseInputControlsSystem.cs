using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class InitBaseInputControlsSystem : BaseEcsVisitable, IEcsPreInitSystem, IEcsDestroySystem
    {
        readonly EcsWorldInject _defaultWorld = default;
        
        readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        
        readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        
        private BaseControls _baseControls;

        public void PreInit(IEcsSystems systems)
        {
            var baseInputControlsEntity = _defaultWorld.Value.NewEntity(); 
            ref var baseInputControlsComp = ref _baseInputControlsPool.Value.Add(baseInputControlsEntity);
            baseInputControlsComp.Value = new BaseControls();
            baseInputControlsComp.Value.Enable();
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