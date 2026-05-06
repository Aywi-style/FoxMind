using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Включает состояние, при котором атака владеет перемещением сущности.
    /// </summary>
    public class ApplyAttackMovementLockSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AttackMovementLockRequest>, Exc<AttackMovementLockComp>> _nonLockedFilter = default;

        private readonly EcsPoolInject<AttackMovementLockComp> _attackMovementLockPool = default;
        
        public void Run(IEcsSystems systems)
        {
            if (_nonLockedFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var nonLockedEntity in _nonLockedFilter.Value)
            {
                _attackMovementLockPool.Value.Add(nonLockedEntity);
            }
        }
    }
}
