using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Components;
using UnityEngine;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Battle.MonoBehaviours
{
    public class HitBoxMb : MonoBehaviour
    {
        [SerializeField] private BaseEntityBaker baseEntityBaker;

        [SerializeField] private Collider[] _hitColliders;
        [SerializeField] private bool _useEcsOverlap = true;
        [SerializeField] private LayerMask _hurtBoxLayerMask = ~0;
        [ReadOnly, ShowInInspector] private int _currentBaseDamage = 0;
        [ReadOnly, ShowInInspector] private float _currentBaseCritChance = 0;
        [ReadOnly, ShowInInspector] private float _currentBaseCritMultiplier = 0;
        [ReadOnly, ShowInInspector] private HitReactionType _currentHitReactionType = HitReactionType.None;
        [ReadOnly, ShowInInspector] private Vector2 _currentReactionVelocity = Vector2.zero;
        [ReadOnly, ShowInInspector] private float _currentHitReactionDuration = 0;
        
        public int CurrentBaseDamage => _currentBaseDamage;
        public float CurrentBaseCritChance => _currentBaseCritChance;
        public float CurrentBaseCritMultiplier => _currentBaseCritMultiplier;
        public HitReactionType CurrentHitReactionType => _currentHitReactionType;
        public Vector2 CurrentReactionVelocity => _currentReactionVelocity;
        public float CurrentHitReactionDuration => _currentHitReactionDuration;

        private bool _isEnabled;
        [ReadOnly, ShowInInspector]private readonly HashSet<EcsPackedEntityWithWorld> _hitThisEnable = new HashSet<EcsPackedEntityWithWorld>();

        public EcsPackedEntityWithWorld PackedEntity => baseEntityBaker.PackedEntity;
        public IReadOnlyList<Collider> HitColliders => _hitColliders;
        public bool UseEcsOverlap => _useEcsOverlap;
        public LayerMask HurtBoxLayerMask => _hurtBoxLayerMask;

        private void Start()
        {
            Disable();
        }

        public void Enable(
            int baseDamageValue,
            float baseCritChance,
            float baseCritMultiplier,
            HitReactionType hitReactionType,
            Vector2 reactionVelocity,
            float hitReactionDuration)
        {
            _hitThisEnable.Clear();
            
            _currentBaseDamage = baseDamageValue;
            _currentBaseCritChance = baseCritChance;
            _currentBaseCritMultiplier = baseCritMultiplier;
            _currentHitReactionType = hitReactionType;
            _currentReactionVelocity = reactionVelocity;
            _currentHitReactionDuration = hitReactionDuration > 0f ? hitReactionDuration : 0.35f;
            
            foreach (var hitCollider in _hitColliders)
            {
                hitCollider.enabled = true;
            }

            _isEnabled = true;
        }

        public void Disable()
        {
            _hitThisEnable.Clear();
            
            foreach (var hitCollider in _hitColliders)
            {
                hitCollider.enabled = false;
            }

            _currentHitReactionType = HitReactionType.None;
            _currentReactionVelocity = Vector2.zero;
            _currentHitReactionDuration = 0;
            _isEnabled = false;
        }

        public bool IsEnabled()
        {
            return _isEnabled;
        }
        
        public bool TryRegisterHit(EcsPackedEntityWithWorld target)
        {
            if (_hitThisEnable.Contains(target))
            {
                return false;
            }
            
            _hitThisEnable.Add(target);
            return true;
        }
        
        /*private void OnTriggerEnter(Collider other)
        {
            if (_useEcsOverlap)
            {
                return;
            }
            
            if (((1 << other.gameObject.layer) & _hurtBoxLayerMask.value) == 0)
            {
                return;
            }
            
            if (other.TryGetComponent<HurtBoxMb>(out var component) == false)
            {
                return;
            }

            if (_isEnabled == false)
            {
                return;
            }

            if (component.PackedEntity.Unpack(out var world, out var entity) == false)
            {
                return;
            }
            
            if (TryRegisterHit(component.PackedEntity) == false)
            {
                return;
            }

            Debug.Log($"Collided with: {other}, world: {world}, entity: {entity}");
            var damageRequestEntity = world.NewEntity();
            ref var causeDamageRequest = ref world.GetPool<CauseDamageRequest>().Add(damageRequestEntity);
            causeDamageRequest.From = PackedEntity;
            causeDamageRequest.To = component.PackedEntity;
            causeDamageRequest.BaseDamage = _currentBaseDamage;
            causeDamageRequest.BaseCritChance = _currentBaseCritChance;
            causeDamageRequest.BaseCritMultiplier = _currentBaseCritMultiplier;
        }*/
    }
}
