using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.Movement.Components.FullFeature;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using FoxMind.Code.Runtime.Core.Stats.Features;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Entities
{
    public class PlayerBaseEntityBaker : BaseEntityBaker
    {
        [SerializeField] private CombinableComp _combinableComp;
        [SerializeField] private CameraFollowMeTransformComp _cameraFollowMeTransformComp;
        [SerializeField] private WeaponComp _weaponComp;
        [SerializeField] private UnitStatsComp unitStatsComp;
        
        [SerializeField] private AnimancerComp _animancerComp;
        [SerializeField] private AnimatorComp _animatorComp;
        [SerializeField] private PushBoxCapsuleComp _pushBoxCapsuleComp;
        [SerializeField] private PlayerControlledComp _playerControlledComp;
        [SerializeField] private RegisterMotionAnimationRequest _registerMotionAnimationRequest;
        [SerializeField] private RegisterMoveableRequest _registerMoveableRequest;
        [SerializeField] private MoveableBehavioursComp _moveableBehavioursComp;
        [SerializeField] private MotionAnimationComp _motionAnimationComp;
        [SerializeField] private JumpableComp _jumpableComp;
        [SerializeField] private SlayerJetCharacterControllerComp _slayerJetCharacterControllerComp;
        [SerializeField] private MoveableComp _moveableComp;
        [SerializeField] private TransformComp _transformComp;
        [SerializeField] private RigidBodyComp _rigidBodyComp;

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
                _combinableComp,
                _cameraFollowMeTransformComp,
                _weaponComp,
                unitStatsComp,
                _animancerComp,
                _animatorComp,
                _pushBoxCapsuleComp,
                _playerControlledComp,
                _registerMotionAnimationRequest,
                _registerMoveableRequest,
                _moveableBehavioursComp,
                _motionAnimationComp,
                _jumpableComp,
                _slayerJetCharacterControllerComp,
                _moveableComp,
                _transformComp,
                _rigidBodyComp
            };
        }
    }
}