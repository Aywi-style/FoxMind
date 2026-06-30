using System;
using FoxMind.Code.Runtime.Core.Battle.Attack.Enums;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    /// <summary>
    /// Временное движение цели во время hit reaction. Обычный player/AI input на время реакции не применяется.
    /// </summary>
    [Serializable]
    public class HitReactionMovementBehaviour : IMovementBehaviour
    {
        [SerializeField] private KinematicCharacterMotor motor;
        
        [Header("Hit Reaction Movement")]
        [SerializeField] private float defaultAirKnockdownSpeed = 20f;
        [SerializeField] private float orientationSharpness = 20f;

        private HitReactionFlags _reactionFlags;
        private Vector3 _worldReactionVelocity;
        private bool _waitForGroundBeforeTimer;
        private bool _forceUngroundRequested;
        private Vector3 _lookInputVector;

        public void Initialize(KinematicCharacterMotor motor)
        {
            this.motor = motor;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
            _worldReactionVelocity = Vector3.zero;
            _lookInputVector = Vector3.zero;
            _forceUngroundRequested = false;
        }

        public void Configure(HitReactionFlags reactionType, Vector3 worldReactionVelocity, bool waitForGroundBeforeTimer)
        {
            var isNewReaction = _reactionFlags != reactionType ||
                                _worldReactionVelocity != worldReactionVelocity ||
                                _waitForGroundBeforeTimer != waitForGroundBeforeTimer;

            _reactionFlags = reactionType;
            _worldReactionVelocity = worldReactionVelocity;
            _waitForGroundBeforeTimer = waitForGroundBeforeTimer;

            if (isNewReaction && reactionType == HitReactionFlags.Launch)
            {
                _forceUngroundRequested = true;
            }
        }

        public void Update()
        {
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
        }

        public void SetLookDirection(Vector3 normalizedLookDirection)
        {
            _lookInputVector = normalizedLookDirection;
        }

        public float GetMaxSpeed()
        {
            return float.MaxValue;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector.sqrMagnitude <= float.Epsilon || orientationSharpness <= 0f)
            {
                return;
            }

            var smoothedLookDirection = Vector3.Slerp(
                motor.CharacterForward,
                _lookInputVector,
                1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

            currentRotation = Quaternion.LookRotation(smoothedLookDirection, motor.CharacterUp);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (_reactionFlags)
            {
                case HitReactionFlags.Knockdown when _waitForGroundBeforeTimer:
                    currentVelocity = Vector3.down * GetAirKnockdownSpeed();
                    break;
                case HitReactionFlags.Knockdown:
                    currentVelocity = Vector3.zero;
                    break;
                case HitReactionFlags.AttackBase:
                case HitReactionFlags.Knockback:
                case HitReactionFlags.Launch:
                    if (_forceUngroundRequested)
                    {
                        motor.ForceUnground(0.1f);
                        _forceUngroundRequested = false;
                    }

                    currentVelocity = _worldReactionVelocity;
                    break;
                default:
                    currentVelocity = Vector3.zero;
                    break;
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        private float GetAirKnockdownSpeed()
        {
            var configuredVerticalSpeed = Mathf.Abs(_worldReactionVelocity.y);
            return configuredVerticalSpeed > 0f ? configuredVerticalSpeed : defaultAirKnockdownSpeed;
        }
    }
}
