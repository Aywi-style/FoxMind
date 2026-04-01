using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
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

                combinableComp.AvailableCombos ??= new List<ComboConfig_v2>();
                
                combinableComp.AvailableCombos.Clear();

                var isWindowForCombo = _cachedTime >= inComboComp.NextComboWindowStart && _cachedTime <= inComboComp.NextComboWindowEnd;
                var timeToComboWindow = inComboComp.NextComboWindowStart - _cachedTime;
                
                foreach (var comboConfig in inComboComp.ComboConfig.NextCombos)
                {
                    var isBufferBeforeComboWindow = timeToComboWindow >= 0 && timeToComboWindow <= comboConfig.LeadTime;
                    
                    // We allow combos either during the active window or slightly before it (LeadTime buffer).
                    if (isWindowForCombo || isBufferBeforeComboWindow)
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

                combinableComp.AvailableCombos ??= new List<ComboConfig_v2>();
                
                combinableComp.AvailableCombos.Clear();

                foreach (var comboConfig in combinableComp.CombosAssembly.OpenerCombosConfigs_v2)
                {
                    combinableComp.AvailableCombos.Add(comboConfig);
                }

            }
        }
    }
}
