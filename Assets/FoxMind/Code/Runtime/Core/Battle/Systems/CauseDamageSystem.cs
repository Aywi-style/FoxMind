using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Система принимает в себя CauseDamageRequest и уменьшает значение EnergyComp для целевой сущности
    /// </summary>
    public class CauseDamageSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<CauseDamageRequest>> _causeDamageRequestFilter = default;

        private readonly EcsPoolInject<CauseDamageRequest> _causeDamageRequestPool = default;
        private readonly EcsPoolInject<EnergyComp> _energyPool = default;
        private readonly EcsPoolInject<DeathRequest> _deathRequestPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var requestEntity in _causeDamageRequestFilter.Value)
            {
                ref var causeDamageRequest = ref _causeDamageRequestPool.Value.Get(requestEntity);

                if (causeDamageRequest.To.Unpack(out var toWorld, out var targetEntity) == false)
                {
                    continue;
                }

                if (causeDamageRequest.From.Unpack(out var fromWorld, out var attackerEntity) == false)
                {
                    continue;
                }

                if (_energyPool.Value.Has(targetEntity) == false)
                {
                    continue;
                }

                if (_inAttackPool.Value.Has(attackerEntity) == false)
                {
                    continue;
                }

                ref var targetEnergyComp = ref _energyPool.Value.Get(targetEntity);
                ref var attackerInAttackComp = ref _inAttackPool.Value.Get(attackerEntity);

                if (targetEnergyComp.CurrentValue <= 0)
                {
                    continue;
                }
                
                targetEnergyComp.CurrentValue -= attackerInAttackComp.AttackConfig.DamageValue;

                if (targetEnergyComp.CurrentValue <= 0)
                {
                    targetEnergyComp.CurrentValue = 0;

                    _deathRequestPool.Value.Add(_world.Value.NewEntity()).For = _world.Value.PackEntity(targetEntity);
                }
            }
        }
    }
}