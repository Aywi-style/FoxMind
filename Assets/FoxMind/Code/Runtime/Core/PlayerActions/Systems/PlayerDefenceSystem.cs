using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.Stats.Features;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Mathematics;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerDefenceSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _inputControlsFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, MoveableComp, UnitStatsComp>> _controlledTransformFilter = default;
        
        private readonly EcsPoolInject<BaseInputControlsComp> _inputControlsPool = default;
        private readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        private readonly EcsPoolInject<UnitStatsComp> _unitStatsPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _inputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _inputControlsPool.Value.Get(inputControlsEntity);
                if (inputControlsComp.Value.GeneralMap.Defence.WasPressedThisFrame() == false)
                {
                    return;
                }

                if (inputControlsComp.InputMoveDirection == Vector2.zero)
                {
                    HandleParry();
                }
                else
                {
                    HandleDash();
                }
            }
        }

        private void HandleParry()
        {
            
        }

        private void HandleDash()
        {
            foreach (var transformEntity in _controlledTransformFilter.Value)
            {
                ref var moveable = ref _moveablePool.Value.Get(transformEntity);
                ref var unitStats = ref _unitStatsPool.Value.Get(transformEntity);

                moveable.CustomCharacterController.SetDashRequest(moveable.NormalizedMoveDirection, unitStats);
                
                Debug.Log("Блинк");
            }
        }
    }
}