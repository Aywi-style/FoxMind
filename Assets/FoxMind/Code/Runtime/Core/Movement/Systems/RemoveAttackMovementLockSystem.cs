using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Снимает состояние, при котором атака владеет перемещением сущности.
    /// </summary>
    public class RemoveAttackMovementLockSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AttackMovementUnlockRequest, AttackMovementLockComp>> _lockedFilter = default;

        private readonly EcsPoolInject<AttackMovementLockComp> _attackMovementLockPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_lockedFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var lockedEntity in _lockedFilter.Value)
            {
                _attackMovementLockPool.Value.Del(lockedEntity);
            }
        }
    }
}
