using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking.Systems
{
    public class TrackInputtedAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp, InputAttackEvent>> _inputDashFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, InputtedAttackComp>> _comboFilter = default;

        private readonly EcsPoolInject<InputtedAttackComp> _inputtedAttackPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_inputDashFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var comboEntity in _comboFilter.Value)
            {
                ref var inputtedAttackComp = ref _inputtedAttackPool.Value.Get(comboEntity);
                inputtedAttackComp.LastPress = Time.time;
            }
        }
    }
}