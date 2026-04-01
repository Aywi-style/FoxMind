using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Entities
{
    public class CameraEntityBaker : BaseEntityBaker
    {
        [SerializeField] private CameraComp _cameraComp;
        [SerializeField] private TransformComp _transformComp;
        
        private List<IEntityFeature> _features;

        protected override void OnValidate()
        {
            return;
        }

        protected override EntityTemplateConfig GetConfig()
        {
            return null;
        }

        protected override List<IEntityFeature> GetFeatures()
        {
            return _features;
        }

        protected override void OnPreInit()
        {
            _features = new List<IEntityFeature>
            {
                _cameraComp,
                _transformComp
            };
        }
    }
}