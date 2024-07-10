using System;
using System.Collections.Generic;
using System.Linq;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours
{
    public class EntityBaker : MonoBehaviour
    {
        [SerializeField] private int _entity;
        [field: SerializeField] public EcsPackedEntityWithWorld PackedEntity { get; private set; }

        [field: SerializeField] public EntityTemplateConfig FeaturesConfig;
        [field: SerializeReference] public List<IEntityFeature> Features = new List<IEntityFeature>();

        private IEnumerable<IEntityFeature> _entityFeatures;

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = Features.Count - 1; i >= 0; i--)
            {
                if (Features[i] == null)
                {
                    Features.Remove(Features[i]);
                }
            }
        }
#endif

        public void Init(EcsWorld world)
        {
            _entityFeatures = FeaturesConfig != null ? FeaturesConfig.Concat(Features) : null;

            if (_entityFeatures != null)
            {
                var entity = world.NewEntity();
                _entity = entity;
                PackedEntity = world.PackEntityWithWorld(entity);
                
                foreach (var feature in _entityFeatures)
                {
                    feature.Compose(world, entity);
                }
            }
        }
    }
}