using FoxMind.Code.Runtime.Core.Battle.Combo.Features;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Core.Systems
{
    public class CleanUpAfterAttackSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<CombinableComp>, Exc<InAttackComp>> _wasInComboFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combinablePool = default;

        private float _cachedTime;
        private bool _isDefencePressed;
        private bool _isJumpPressed;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _wasInComboFilter.Value)
            {
                ref var combinableComp = ref _combinablePool.Value.Get(entity);
                combinableComp.CurrentCombo = null;
            }
        }
    }
}