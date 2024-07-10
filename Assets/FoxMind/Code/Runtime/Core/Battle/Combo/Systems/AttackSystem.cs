using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class AttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerInputComp>> _playerInputFilter = default;
        private readonly EcsFilterInject<Inc<AnimancerComp, PlayerControlledComp>> _attackableFilter = default;

        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;
        private readonly EcsPoolInject<PlayerInputComp> _playerInputPool = default;

        private Vector3 _cachedMoveVelocity;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var inputEntity in _playerInputFilter.Value)
            {
                ref var input = ref _playerInputPool.Value.Get(inputEntity);

                if (input.Attack == false)
                {
                    return;
                }
                
                foreach (var attackableEntity in _attackableFilter.Value)
                {
                    ref var animacer = ref _animancerPool.Value.Get(attackableEntity);

                    animacer.Value.Play(animacer.Clip).Time = 0;
                }
            }
        }
    }
}