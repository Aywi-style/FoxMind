using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.InputTracking.Systems
{
    public class RegisterTrackingForComboSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedMeleeAttackComp>> _nonInputtedMeleeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedRangeAttackComp>> _nonInputtedRangeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedDoubleMeleeAttackComp>> _nonInputtedDoubleMeleeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedLongMeleeAttackComp>> _nonInputtedLongMeleeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedDoubleRangeAttackComp>> _nonInputtedDoubleRangeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedLongRangeAttackComp>> _nonInputtedLongRangeAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedDashComp>> _nonInputtedDashFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedJumpComp>> _nonInputtedJumpFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedForwardMoveComp>> _nonInputtedForwardMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedBackwardMoveComp>> _nonInputtedBackwardMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedLeftMoveComp>> _nonInputtedLeftMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedRightMoveComp>> _nonInputtedRightMoveFilter = default;

        private readonly EcsPoolInject<InputtedMeleeAttackComp> _inputtedMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedRangeAttackComp> _inputtedRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleMeleeAttackComp> _inputtedDoubleMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongMeleeAttackComp> _inputtedLongMeleeAttackPool = default;
        private readonly EcsPoolInject<InputtedDoubleRangeAttackComp> _inputtedDoubleRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedLongRangeAttackComp> _inputtedLongRangeAttackPool = default;
        private readonly EcsPoolInject<InputtedDashComp> _inputtedDashPool = default;
        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;

        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;

        public void Run(IEcsSystems systems)
        {
            // input melee attack
            foreach (var nonInputtedMeleeAttackEntity in _nonInputtedMeleeAttackFilter.Value)
            {
                _inputtedMeleeAttackPool.Value.Add(nonInputtedMeleeAttackEntity).LastPress = int.MinValue;
            }
            // input range attack
            foreach (var nonInputtedRangeAttackEntity in _nonInputtedRangeAttackFilter.Value)
            {
                _inputtedRangeAttackPool.Value.Add(nonInputtedRangeAttackEntity).LastPress = int.MinValue;
            }
            // input double melee attack
            foreach (var nonInputtedDoubleMeleeAttackEntity in _nonInputtedDoubleMeleeAttackFilter.Value)
            {
                _inputtedDoubleMeleeAttackPool.Value.Add(nonInputtedDoubleMeleeAttackEntity).LastPress = int.MinValue;
            }
            // input long melee attack
            foreach (var nonInputtedLongMeleeAttackEntity in _nonInputtedLongMeleeAttackFilter.Value)
            {
                _inputtedLongMeleeAttackPool.Value.Add(nonInputtedLongMeleeAttackEntity).LastPress = int.MinValue;
            }
            // input double range attack
            foreach (var nonInputtedDoubleRangeAttackEntity in _nonInputtedDoubleRangeAttackFilter.Value)
            {
                _inputtedDoubleRangeAttackPool.Value.Add(nonInputtedDoubleRangeAttackEntity).LastPress = int.MinValue;
            }
            // input long range attack
            foreach (var nonInputtedLongRangeAttackEntity in _nonInputtedLongRangeAttackFilter.Value)
            {
                _inputtedLongRangeAttackPool.Value.Add(nonInputtedLongRangeAttackEntity).LastPress = int.MinValue;
            }
            // input dash
            foreach (var nonInputtedDashEntity in _nonInputtedDashFilter.Value)
            {
                _inputtedDashPool.Value.Add(nonInputtedDashEntity).LastPress = int.MinValue;
            }
            // input jump
            foreach (var nonInputtedJumpEntity in _nonInputtedJumpFilter.Value)
            {
                _inputtedJumpPool.Value.Add(nonInputtedJumpEntity).LastPress = int.MinValue;
            }
            
            // input movement
            foreach (var nonInputtedForwardMoveEntity in _nonInputtedForwardMoveFilter.Value)
            {
                _inputtedForwardMovePool.Value.Add(nonInputtedForwardMoveEntity).LastPress = int.MinValue;
            }
            
            foreach (var nonInputtedBackwardMoveEntity in _nonInputtedBackwardMoveFilter.Value)
            {
                _inputtedBackwardMovePool.Value.Add(nonInputtedBackwardMoveEntity).LastPress = int.MinValue;
            }
            
            foreach (var nonInputtedLeftMoveEntity in _nonInputtedLeftMoveFilter.Value)
            {
                _inputtedLeftMovePool.Value.Add(nonInputtedLeftMoveEntity).LastPress = int.MinValue;
            }
            
            foreach (var nonInputtedRightMoveEntity in _nonInputtedRightMoveFilter.Value)
            {
                _inputtedRightMovePool.Value.Add(nonInputtedRightMoveEntity).LastPress = int.MinValue;
            }
        }
    }
}
