using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система отрабатывает прыжки для Slayer Jet
    /// </summary>
    public class JumpSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputJumpEvent>> _jumpEventFilter = default;
        private readonly EcsFilterInject<Inc<MoveableComp, JumpableComp, SlayerJetCharacterControllerComp>> _jumpFilter = default;

        private readonly EcsPoolInject<JumpableComp> _jumpablePool = default;
        private readonly EcsPoolInject<SlayerJetCharacterControllerComp> _slayerJetCharacterControllerPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_jumpEventFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var movableEntity in _jumpFilter.Value)
            {
                ref var jumpableComp = ref _jumpablePool.Value.Get(movableEntity);
                ref var slayerJetCharacterControllerComp = ref _slayerJetCharacterControllerPool.Value.Get(movableEntity);
            
                slayerJetCharacterControllerComp.Value.SetJumpRequest();
            }
        }
    }
}