using System;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Input.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Input.Systems
{
    public class FullUtilityInputHistorySystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputControlsFilter = default;
        
        private readonly EcsPoolInject<BaseInputControlsComp> _baseInputControlsPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var inputControlsEntity in _baseInputControlsFilter.Value)
            {
                ref var inputControlsComp = ref _baseInputControlsPool.Value.Get(inputControlsEntity);
            
                if (inputControlsComp.Value.GeneralMap.TargetLock.WasPressedThisFrame())
                {
                    inputControlsComp.UtilityInputHistory.Push(UtilityInputData.Create(UtilityInputType.Targeting, PressType.Started));
                }

                if (inputControlsComp.Value.GeneralMap.TargetLock.WasReleasedThisFrame())
                {
                    inputControlsComp.UtilityInputHistory.Push(UtilityInputData.Create(UtilityInputType.Targeting, PressType.Cancelled));
                }
            }
        }
    }
}
