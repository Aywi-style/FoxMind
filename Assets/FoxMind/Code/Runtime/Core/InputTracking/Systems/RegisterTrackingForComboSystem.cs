using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.InputTracking.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.InputTracking.Systems
{
    public class RegisterTrackingForComboSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedAttackComp>> _nonInputtedAttackFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedDashComp>> _nonInputtedDashFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp>, Exc<InputtedJumpComp>> _nonInputtedJumpFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedForwardMoveComp>> _nonInputtedForwardMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedBackwardMoveComp>> _nonInputtedBackwardMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedLeftMoveComp>> _nonInputtedLeftMoveFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>, Exc<InputtedRightMoveComp>> _nonInputtedRightMoveFilter = default;

        private readonly EcsPoolInject<InputtedAttackComp> _inputtedAttackPool = default;
        private readonly EcsPoolInject<InputtedDashComp> _inputtedDashPool = default;
        private readonly EcsPoolInject<InputtedJumpComp> _inputtedJumpPool = default;

        private readonly EcsPoolInject<InputtedForwardMoveComp> _inputtedForwardMovePool = default;
        private readonly EcsPoolInject<InputtedBackwardMoveComp> _inputtedBackwardMovePool = default;
        private readonly EcsPoolInject<InputtedLeftMoveComp> _inputtedLeftMovePool = default;
        private readonly EcsPoolInject<InputtedRightMoveComp> _inputtedRightMovePool = default;

        public void Run(IEcsSystems systems)
        {
            // input attack
            foreach (var nonInputtedAttackEntity in _nonInputtedAttackFilter.Value)
            {
                _inputtedAttackPool.Value.Add(nonInputtedAttackEntity).LastPress = -10;
            }
            // input dash
            foreach (var nonInputtedDashEntity in _nonInputtedDashFilter.Value)
            {
                _inputtedDashPool.Value.Add(nonInputtedDashEntity).LastPress = -10;
            }
            // input jump
            foreach (var nonInputtedJumpEntity in _nonInputtedJumpFilter.Value)
            {
                _inputtedJumpPool.Value.Add(nonInputtedJumpEntity).LastPress = -10;
            }
            
            // input movement
            foreach (var nonInputtedForwardMoveEntity in _nonInputtedForwardMoveFilter.Value)
            {
                _inputtedForwardMovePool.Value.Add(nonInputtedForwardMoveEntity).LastPress = -10;
            }
            
            foreach (var nonInputtedBackwardMoveEntity in _nonInputtedBackwardMoveFilter.Value)
            {
                _inputtedBackwardMovePool.Value.Add(nonInputtedBackwardMoveEntity).LastPress = -10;
            }
            
            foreach (var nonInputtedLeftMoveEntity in _nonInputtedLeftMoveFilter.Value)
            {
                _inputtedLeftMovePool.Value.Add(nonInputtedLeftMoveEntity).LastPress = -10;
            }
            
            foreach (var nonInputtedRightMoveEntity in _nonInputtedRightMoveFilter.Value)
            {
                _inputtedRightMovePool.Value.Add(nonInputtedRightMoveEntity).LastPress = -10;
            }
        }
    }
}