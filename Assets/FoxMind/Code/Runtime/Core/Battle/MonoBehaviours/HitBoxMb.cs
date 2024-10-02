using UnityEngine;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.MonoBehaviours
{
    public class HitBoxMb : MonoBehaviour
    {
        [SerializeField] private EntityBaker _entityBaker;

        public EcsPackedEntityWithWorld PackedEntity => _entityBaker.PackedEntity;
        
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
        }
    }
}