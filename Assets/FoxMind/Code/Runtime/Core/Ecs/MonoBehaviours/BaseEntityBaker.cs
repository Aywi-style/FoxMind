using System;
using System.Collections.Generic;
using System.Linq;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours
{
    public abstract class BaseEntityBaker : MonoBehaviour
    {
        [ReadOnly, SerializeField] private int _entity;
        [field: SerializeField] public EcsPackedEntityWithWorld PackedEntity { get; protected set; }

        private IEnumerable<IEntityFeature> _entityFeatures;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            var features = GetFeatures();
            for (int i = features.Count - 1; i >= 0; i--)
            {
                if (features[i] == null)
                {
                    features.Remove(features[i]);
                }
            }
        }
#endif

        protected abstract EntityTemplateConfig GetConfig();

        protected abstract List<IEntityFeature> GetFeatures();

        protected abstract void OnPreInit();
        
        public void Init(EcsWorld world)
        {
            OnPreInit();
            
            try
            {
                var featuresConfig = GetConfig();
                var features = GetFeatures();
                
                if (featuresConfig != null && features != null)
                {
                    _entityFeatures = featuresConfig.Concat(features);
                }
                else if (featuresConfig != null)
                {
                    _entityFeatures = featuresConfig;
                }
                else if (features != null && features.Count > 0)
                {
                    _entityFeatures = features;
                }

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
            catch (Exception e)
            {
                Debug.LogError($"In {name} null comp!");
            }
        }
    }
}