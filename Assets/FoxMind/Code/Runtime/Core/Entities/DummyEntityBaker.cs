using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Fractions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.Stats.Features;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Entities
{
    public class DummyEntityBaker : BaseEntityBaker
    {
        [SerializeField] private FractionComp _fractionComp;
        [SerializeField] private VisualHolderComp _visualHolderComp;
        [SerializeField] private UnitStatsComp _unitStatsComp;
        [SerializeField] private TargetFindableComp _targetFindableComp;
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
                _targetFindableComp,
                _transformComp,
                _fractionComp,
                _visualHolderComp,
                _unitStatsComp
            };
        }
    }
}