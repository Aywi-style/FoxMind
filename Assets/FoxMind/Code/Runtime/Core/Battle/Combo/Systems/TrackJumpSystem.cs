using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class TrackJumpSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp, InputDashEvent>> _inputDashFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, CombinableComp, InputtedJumpComp>> _comboFilter = default;

        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var comboEntity in _comboFilter.Value)
            {
                ref var inputtedJumpComp = ref _inputtedJumpPool.Value.Get(comboEntity);
                inputtedJumpComp.LastPress = Time.time;
            }
        }
    }
}