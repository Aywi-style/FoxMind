using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class CatchInputAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<InputAttackEvent>> _inputAttackEventFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<SelfDefineWhatComboNeedToDoRequest>> _playerControlledFilter = default;

        private readonly EcsPoolInject<SelfDefineWhatComboNeedToDoRequest> _comboAttackRequestPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_inputAttackEventFilter.Value.GetEntitiesCount() == 0)
            {
                return;
            }
            
            foreach (var playerControlledEntity in _playerControlledFilter.Value)
            {
                _comboAttackRequestPool.Value.Add(playerControlledEntity);
            }
        }
    }
}