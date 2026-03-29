using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours
{
    public class EntityBaker : BaseEntityBaker
    {
        [field: SerializeField] public EntityTemplateConfig FeaturesConfig;
        [field: SerializeReference] public List<IEntityFeature> Features = new List<IEntityFeature>();
        
        protected override EntityTemplateConfig GetConfig()
        {
            return FeaturesConfig;
        }

        protected override List<IEntityFeature> GetFeatures()
        {
            return Features;
        }

        protected override void OnPreInit()
        {
            
        }
    }
}