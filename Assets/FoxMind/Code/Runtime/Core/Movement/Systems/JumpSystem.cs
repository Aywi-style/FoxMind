using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Обрабатывает запрос прыжка и передаёт его в CustomCharacterController сущности.
    /// </summary>
    public class JumpSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputJumpEvent>> _jumpEventFilter = default;
        private readonly EcsFilterInject<Inc<MoveableComp, JumpableComp, CharacterControllerComp>> _jumpFilter = default;

        private readonly EcsPoolInject<JumpableComp> _jumpablePool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;

        public void Run(IEcsSystems systems)
        {
            if (_jumpEventFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var movableEntity in _jumpFilter.Value)
            {
                ref var jumpableComp = ref _jumpablePool.Value.Get(movableEntity);
                ref var characterControllerComp = ref _characterControllerPool.Value.Get(movableEntity);
            
                characterControllerComp.Value.SetJumpRequest();
            }
        }
    }
}
