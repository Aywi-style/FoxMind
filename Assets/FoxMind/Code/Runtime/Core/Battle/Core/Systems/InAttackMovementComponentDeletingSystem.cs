using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Удаляет состояние движения атаки, когда заканчивается настроенное movement window.
    /// </summary>
    public class InAttackMovementComponentDeletingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InAttackMovementComp>> _inAttackMovementFilter = default;
        private readonly EcsPoolInject<InAttackMovementComp> _inAttackMovementPool = default;

        public void Run(IEcsSystems systems)
        {
            var time = Time.time;
            
            foreach (var entity in _inAttackMovementFilter.Value)
            {
                ref var inAttackMovement = ref _inAttackMovementPool.Value.Get(entity);
                if (time < inAttackMovement.EndTime)
                {
                    continue;
                }

                _inAttackMovementPool.Value.Del(entity);
            }
        }
    }
}
