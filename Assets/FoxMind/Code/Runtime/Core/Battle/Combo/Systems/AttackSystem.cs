using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class AttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InputAttackEvent>> _inputAttackFilter = default;
        private readonly EcsFilterInject<Inc<AnimancerComp, PlayerControlledComp>> _attackableFilter = default;

        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            if (_inputAttackFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var attackableEntity in _attackableFilter.Value)
            {
                ref var animacer = ref _animancerPool.Value.Get(attackableEntity);

                //animacer.Value.Play(animacer.Clip).Time = 0;
            }
        }
    }
}