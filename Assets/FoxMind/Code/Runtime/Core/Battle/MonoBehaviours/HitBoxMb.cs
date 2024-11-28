using System;
using FoxMind.Code.Runtime.Core.Battle.Components;
using UnityEngine;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Battle.MonoBehaviours
{
    public class HitBoxMb : MonoBehaviour
    {
        [SerializeField] private EntityBaker _entityBaker;

        [SerializeField] private Collider[] _hitColliders;

        private bool _isEnabled;

        public EcsPackedEntityWithWorld PackedEntity => _entityBaker.PackedEntity;

        private void Start()
        {
            Disable();
        }

        public void Enable()
        {
            foreach (var hitCollider in _hitColliders)
            {
                hitCollider.enabled = true;
            }

            _isEnabled = true;
        }

        public void Disable()
        {
            foreach (var hitCollider in _hitColliders)
            {
                hitCollider.enabled = false;
            }

            _isEnabled = false;
        }

        public bool IsEnabled()
        {
            return _isEnabled;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<HurtBoxMb>(out var component) == false)
            {
                return;
            }

            if (component.PackedEntity.Unpack(out var world, out var entity) == false)
            {
                return;
            }
            
            Debug.Log($"Collided with: {other}, world: {world}, entity: {entity}");

            ref var causeDamageRequest = ref world.GetPool<CauseDamageRequest>().Add(world.NewEntity());
            causeDamageRequest.From = PackedEntity;
            causeDamageRequest.To = component.PackedEntity;
        }
    }
}