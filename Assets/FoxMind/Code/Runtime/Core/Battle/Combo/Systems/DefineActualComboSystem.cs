using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
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
        
        private readonly EcsFilterInject<Inc<CombinableComp, InComboComp>> _inComboFilter = default;
        private readonly EcsFilterInject<Inc<CombinableComp>, Exc<InComboComp>> _nonInComboFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combinablePool = default;
        private readonly EcsPoolInject<InComboComp> _inComboPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;
        
        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;

            DefineForInCombo();
            DefineForNonInCombo();
        }

        private void DefineForInCombo()
        {
            if (_inComboFilter.Value.GetEntitiesCount() == 0)
            {
                return;
            }
            
            foreach (var inComboEntity in _inComboFilter.Value)
            {
                ref var combinableComp = ref _combinablePool.Value.Get(inComboEntity);
                ref var inComboComp = ref _inComboPool.Value.Get(inComboEntity);

                //combinableComp.AvailableCombos ??= new List<ComboConfig>();
                combinableComp.AvailableCombos ??= new TestClass();
                
                combinableComp.AvailableCombos.Clear();

                var isWindowForCombo = inComboComp.NextComboWindowStart <= _cachedTime && _cachedTime <= inComboComp.NextComboWindowEnd;
                
                foreach (var comboConfig in inComboComp.ComboConfig.NextCombos)
                {
                    if (IsComboAllowedForStance(comboConfig, inComboEntity) == false)
                    {
                        continue;
                    }
                    
                    if (isWindowForCombo)
                    {
                        combinableComp.AvailableCombos.Add(comboConfig);
                    }
                }

            }
        }

        private void DefineForNonInCombo()
        {
            if (_nonInComboFilter.Value.GetEntitiesCount() == 0)
            {
                return;
            }
            
            foreach (var nonInComboEntity in _nonInComboFilter.Value)
            {
                ref var combinableComp = ref _combinablePool.Value.Get(nonInComboEntity);

                //combinableComp.AvailableCombos ??= new List<ComboConfig>();
                combinableComp.AvailableCombos ??= new TestClass();
                
                combinableComp.AvailableCombos.Clear();

                foreach (var comboConfig in combinableComp.CombosAssembly.OpenerCombosConfigs_v2)
                {
                    if (IsComboAllowedForStance(comboConfig, nonInComboEntity) == false)
                    {
                        continue;
                    }

                    combinableComp.AvailableCombos.Add(comboConfig);
                }

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
