using System;
using FoxMind.Code.Runtime.Core.Constants;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    [Serializable]
    public class AirMovementBehaviour : IMovementBehaviour
    {
        [SerializeField] private KinematicCharacterMotor motor;

        [Header("Air Movement")]
        [SerializeField] private float MaxAirMoveSpeed = 10f;
        [SerializeField] private float AirAccelerationSpeed = 5f;
        [SerializeField] private float Drag = 0.1f;
        [SerializeField] private float OrientationSharpness = 10;
        
        [SerializeField] public Vector3 _moveInputVector;
        [SerializeField] public Vector3 _lookInputVector;

        public void Initialize(KinematicCharacterMotor motor)
        {
            this.motor = motor;
        }

        public void Update()
        {
            
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            _moveInputVector = normalizedMoveDirection;
        }

        public void SetLookDirection(Vector3 normalizedLookDirection)
        {
            _lookInputVector = normalizedLookDirection;
        }

        public float GetMaxSpeed()
        {
            return MaxAirMoveSpeed;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                return;
            }
            
            if (_moveInputVector.sqrMagnitude > 0f)
            {
                var targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                // Prevent climbing on un-stable slopes with air movement
                if (motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                    targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, CustomPhysics.Gravity);
                currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
            }
            
            // Gravity
            currentVelocity += CustomPhysics.Gravity * deltaTime;

            // Drag
            currentVelocity *= (1f / (1f + (Drag * deltaTime)));
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

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            
        }
    }
}
