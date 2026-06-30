using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Battle.Combo.Features;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class DefineActualComboSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<CombinableComp>> _combinableFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combinablePool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;
        
        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;

            foreach (var combinableEntity in _combinableFilter.Value)
            {
                ref var combinableComp = ref _combinablePool.Value.Get(combinableEntity);

                combinableComp.AvailableCombos ??= new List<ComboConfig>();
                combinableComp.AvailableCombos.Clear();

                if (_inAttackPool.Value.Has(combinableEntity) == false)
                {
                    DefineOpeners(ref combinableComp, combinableEntity);
                    
                    continue;
                }
                
                ref var inAttackComp = ref _inAttackPool.Value.Get(combinableEntity);

                if (_cachedTime > inAttackComp.EndOfContinuousPart)
                {
                    DefineOpeners(ref combinableComp, combinableEntity);
                    
                    continue;
                }
                
                var isWindowForCombo = inAttackComp.NextComboWindowStart <= _cachedTime && _cachedTime <= inAttackComp.NextComboWindowEnd;
                if (isWindowForCombo == false)
                {
                    continue;
                }
                
                foreach (var comboConfig in combinableComp.CurrentCombo.NextCombos)
                {
                    if (IsComboAllowedForStance(comboConfig, combinableEntity) == false)
                    {
                        continue;
                    }
                    
                    combinableComp.AvailableCombos.Add(comboConfig);
                }
            }
        }

        private void DefineOpeners(ref CombinableComp combinableComp, int combinableEntity)
        {
            foreach (var comboConfig in combinableComp.CombosAssembly.OpenerCombosConfigs_v2)
            {
                if (IsComboAllowedForStance(comboConfig, combinableEntity) == false)
                {
                    continue;
                }

                combinableComp.AvailableCombos.Add(comboConfig);
            }
        }

        private bool IsComboAllowedForStance(ComboConfig comboConfig, int entity)
        {
            if (comboConfig == null)
            {
                return false;
            }

            switch (comboConfig.StanceCondition)
            {
                case ComboStanceCondition.Any:
                    return true;
                case ComboStanceCondition.GroundedOnly:
                    return TryGetIsGrounded(entity, out var isGrounded) && isGrounded;
                case ComboStanceCondition.AirborneOnly:
                    return TryGetIsGrounded(entity, out isGrounded) && isGrounded == false;
                default:
                    return true;
            }
        }

        private bool TryGetIsGrounded(int entity, out bool isGrounded)
        {
            isGrounded = false;

            if (_characterControllerPool.Value.Has(entity) == false)
            {
                return false;
            }

            ref var characterController = ref _characterControllerPool.Value.Get(entity);
            if (characterController.Value == null || characterController.Value.Motor == null)
            {
                return false;
            }

            isGrounded = characterController.Value.Motor.GroundingStatus.IsStableOnGround;
            return true;
        }
    }
}
