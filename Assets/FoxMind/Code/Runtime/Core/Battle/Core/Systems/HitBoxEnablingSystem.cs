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

        private float _cachedTime;
        
        public void Run(IEcsSystems systems)
        {
            _cachedTime = Time.time;
            
            EnableHitBoxes();
            DisableHitBoxes();
        }

        private void EnableHitBoxes()
        {
            foreach (var inAttackWeaponEntity in _inAttackWeaponFilter.Value)
            {
                ref var weaponEntity = ref _weaponPool.Value.Get(inAttackWeaponEntity);
                ref var inAttackComp = ref _inAttackPool.Value.Get(inAttackWeaponEntity);

                if (weaponEntity.HitBoxMb == null || inAttackComp.AttackConfig == null)
                {
                    continue;
                }
                
                var animLength = inAttackComp.AnimationDuration;
                if (animLength <= 0)
                {
                    continue;
                }
                
                var normalizedTime = (_cachedTime - inAttackComp.Start) / animLength;
                
                var hitWindow = inAttackComp.AttackConfig.HitWindow;
                var hitWindowStart = Mathf.Min(hitWindow.x, hitWindow.y);
                var hitWindowEnd = Mathf.Max(hitWindow.x, hitWindow.y);
                var isHitWindow = normalizedTime >= hitWindowStart && normalizedTime <= hitWindowEnd;
                
                if (isHitWindow)
                {
                    if (weaponEntity.HitBoxMb.IsEnabled() == false)
                    {
                        weaponEntity.HitBoxMb.Enable(
                            inAttackComp.AttackConfig.BaseDamage,
                            inAttackComp.AttackConfig.BaseCritChance,
                            inAttackComp.AttackConfig.BaseCritMultiplier,
                            inAttackComp.AttackConfig.HitReactionFlags,
                            inAttackComp.AttackConfig.ReactionVelocity,
                            inAttackComp.AttackConfig.HitReactionDuration
                            );
                    }
                }
                else
                {
                    if (weaponEntity.HitBoxMb.IsEnabled())
                    {
                        weaponEntity.HitBoxMb.Disable();
                    }
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
