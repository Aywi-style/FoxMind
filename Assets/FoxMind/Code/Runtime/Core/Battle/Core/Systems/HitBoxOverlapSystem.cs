using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Battle.Core.Features;
using FoxMind.Code.Runtime.Core.Battle.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// ECS-driven hit detection using collider authoring (melee).
    /// </summary>
    public class HitBoxOverlapSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private const int c_overlapBufferSize = 64;
        private static readonly Collider[] _overlapBuffer = new Collider[c_overlapBufferSize];
        
        private readonly EcsWorldInject _world = default;
        
        private readonly EcsFilterInject<Inc<WeaponComp>> _weaponFilter = default;
        private readonly EcsPoolInject<WeaponComp> _weaponPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var weaponEntity in _weaponFilter.Value)
            {
                ref var weaponComp = ref _weaponPool.Value.Get(weaponEntity);
                var hitBox = weaponComp.HitBoxMb;
                if (hitBox == null || hitBox.IsEnabled() == false || hitBox.UseEcsOverlap == false)
                {
                    continue;
                }

                var hitColliders = hitBox.HitColliders;
                if (hitColliders == null || hitColliders.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < hitColliders.Count; i++)
                {
                    var collider = hitColliders[i];
                    if (collider == null)
                    {
                        continue;
                    }

                    PerformOverlap(hitBox, collider);
                }
            }
        }

        private void PerformOverlap(HitBoxMb hitBox, Collider collider)
        {
            int count = 0;
            
            switch (collider)
            {
                case BoxCollider box:
                {
                    var center = box.transform.TransformPoint(box.center);
                    var halfExtents = Vector3.Scale(box.size * 0.5f, box.transform.lossyScale);
                    var rotation = box.transform.rotation;
                
                count = Physics.OverlapBoxNonAlloc(center, halfExtents, _overlapBuffer, rotation, hitBox.HurtBoxLayerMask.value, QueryTriggerInteraction.Collide);
                    break;
                }
                case SphereCollider sphere:
                {
                    var center = sphere.transform.TransformPoint(sphere.center);
                    var radius = sphere.radius * Mathf.Max(sphere.transform.lossyScale.x, sphere.transform.lossyScale.y, sphere.transform.lossyScale.z);
                
                count = Physics.OverlapSphereNonAlloc(center, radius, _overlapBuffer, hitBox.HurtBoxLayerMask.value, QueryTriggerInteraction.Collide);
                    break;
                }
                case CapsuleCollider capsule:
                {
                    GetCapsulePoints(capsule, out var p1, out var p2, out var radius);
                
                count = Physics.OverlapCapsuleNonAlloc(p1, p2, radius, _overlapBuffer, hitBox.HurtBoxLayerMask.value, QueryTriggerInteraction.Collide);
                    break;
                }
                default:
                {
                    // Fallback: approximate as bounds box
                    var bounds = collider.bounds;
                count = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, _overlapBuffer, Quaternion.identity, hitBox.HurtBoxLayerMask.value, QueryTriggerInteraction.Collide);
                    break;
                }
            }

            for (int i = 0; i < count; i++)
            {
                var other = _overlapBuffer[i];
                if (other == null)
                {
                    continue;
                }

                if (other.TryGetComponent<HurtBoxMb>(out var hurtBox) == false)
                {
                    continue;
                }

                if (hurtBox.PackedEntity.Unpack(out var world, out var entity) == false)
                {
                    continue;
                }

                if (hitBox.PackedEntity.Equals(hurtBox.PackedEntity))
                {
                    continue;
                }

                if (hitBox.TryRegisterHit(hurtBox.PackedEntity) == false)
                {
                    continue;
                }
                
                ref var causeDamageRequest = ref world.GetPool<CauseDamageRequest>().Add(world.NewEntity());
                causeDamageRequest.From = hitBox.PackedEntity;
                causeDamageRequest.To = hurtBox.PackedEntity;
                causeDamageRequest.BaseDamage = hitBox.CurrentBaseDamage;
                causeDamageRequest.BaseCritChance = hitBox.CurrentBaseCritChance;
                causeDamageRequest.BaseCritMultiplier = hitBox.CurrentBaseCritMultiplier;
                causeDamageRequest.HitReactionFlags = hitBox.CurrentHitReactionFlags;
                causeDamageRequest.ReactionVelocity = hitBox.CurrentReactionVelocity;
                causeDamageRequest.HitReactionDuration = hitBox.CurrentHitReactionDuration;
            }
        }

        private static void GetCapsulePoints(CapsuleCollider capsule, out Vector3 p1, out Vector3 p2, out float radius)
        {
            var transform = capsule.transform;
            var scale = transform.lossyScale;
            var center = transform.TransformPoint(capsule.center);

            Vector3 axis;
            float height;
            
            switch (capsule.direction)
            {
                case 0:
                    axis = transform.right;
                    height = capsule.height * scale.x;
                    radius = capsule.radius * Mathf.Max(scale.y, scale.z);
                    break;
                case 2:
                    axis = transform.forward;
                    height = capsule.height * scale.z;
                    radius = capsule.radius * Mathf.Max(scale.x, scale.y);
                    break;
                default:
                    axis = transform.up;
                    height = capsule.height * scale.y;
                    radius = capsule.radius * Mathf.Max(scale.x, scale.z);
                    break;
            }

            var segment = Mathf.Max(0f, height - 2f * radius);
            var offset = axis * (segment * 0.5f);
            
            p1 = center + offset;
            p2 = center - offset;
        }
    }
}
