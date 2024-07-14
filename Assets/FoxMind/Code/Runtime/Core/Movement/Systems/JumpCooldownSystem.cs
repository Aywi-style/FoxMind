using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    public class JumpCooldownSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<JumpableComp>> _jumpFilter = default;

        private readonly EcsPoolInject<JumpableComp> _jumpablePool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var jumpEntity in _jumpFilter.Value)
            {
                ref var jumpable = ref _jumpablePool.Value.Get(jumpEntity);

                if (jumpable.CooldownCurrent <= 0)
                {
                    continue;
                }
                
                jumpable.CooldownCurrent -= Time.deltaTime;

                if (jumpable.CooldownCurrent < 0) jumpable.CooldownCurrent = 0;
            }
        }
    }
}