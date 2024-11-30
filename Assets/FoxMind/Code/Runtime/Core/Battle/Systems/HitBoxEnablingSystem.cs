using System.Diagnostics;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Отвечает за включение и выключение хитбоксов оружия во время атак
    /// </summary>
    public class HitBoxEnablingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InAttackComp, WeaponComp>> _inAttackWeaponFilter = default;
        private readonly EcsFilterInject<Inc<WeaponComp>, Exc<InAttackComp>> _outOfAttackWeaponFilter = default;

        private readonly EcsPoolInject<WeaponComp> _weaponPool = default;
        private readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        
        public void Run(IEcsSystems systems)
        {
            EnableHitBoxes();
            DisableHitBoxes();
        }

        private void EnableHitBoxes()
        {
            foreach (var inAttackWeaponEntity in _inAttackWeaponFilter.Value)
            {
                ref var weaponEntity = ref _weaponPool.Value.Get(inAttackWeaponEntity);

                if (weaponEntity.HitBoxMb.IsEnabled() == false)
                {
                    weaponEntity.HitBoxMb.Enable();
                }
            }
        }

        private void DisableHitBoxes()
        {
            foreach (var outOfAttackWeaponEntity in _outOfAttackWeaponFilter.Value)
            {
                ref var weaponEntity = ref _weaponPool.Value.Get(outOfAttackWeaponEntity);

                if (weaponEntity.HitBoxMb.IsEnabled())
                {
                    weaponEntity.HitBoxMb.Disable();
                }
            }
        }
    }
}