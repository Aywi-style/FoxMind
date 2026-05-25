using System;
using FoxMind.Code.Runtime.Core.Constants;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    [Serializable]
    public class RootMotionAirMovementBehaviour : IMovementBehaviour, IRootMotion
    {
        [SerializeField] private KinematicCharacterMotor motor;

        [field: Header("Root Motion Air Movement")]
        public Vector3 MoveRootMotionVector { get; set; }
        public Quaternion LookRootMotionQuaternion { get; set; }
        [SerializeField] private float OrientationSharpness = 10f;
        [SerializeField] private Vector3 _lookInputVector;
        
        [Header("Animation Parameters / Air Movement")]
        [SerializeField] private float MaxAirMoveSpeed = 10f;
        [SerializeField] private float AirAccelerationSpeed = 5f;
        [SerializeField] private float Drag = 0.1f;
        
        public float ForwardAxisSharpness = 10;
        public float TurnAxisSharpness = 5;
        
        private float _forwardAxis;
        private float _rightAxis;
        private float _targetForwardAxis;
        private float _targetRightAxis;
        
        public void Update()
        {
            _forwardAxis = Mathf.Lerp(_forwardAxis, _targetForwardAxis, 1f - Mathf.Exp(-ForwardAxisSharpness * Time.deltaTime));
            _rightAxis = Mathf.Lerp(_rightAxis, _targetRightAxis, 1f - Mathf.Exp(-TurnAxisSharpness * Time.deltaTime));
        }
        
        public void Initialize(KinematicCharacterMotor motor)
        {
            this.motor = motor;
        }

        public void Enter()
        {
            ClearRootMotion();
        }

        public void Exit()
        {
            ClearRootMotion();
            _lookInputVector = Vector3.zero;
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
            if (_lookInputVector.sqrMagnitude <= float.Epsilon || OrientationSharpness <= 0f)
            {
                return;
            }
            
            var smoothedLookDirection = Vector3.Slerp(
                motor.CharacterForward,
                _lookInputVector,
                1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
                
            currentRotation = Quaternion.LookRotation(smoothedLookDirection, motor.CharacterUp);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (deltaTime > 0f && MoveRootMotionVector.sqrMagnitude > float.Epsilon)
            {
                currentVelocity = MoveRootMotionVector / deltaTime;
                ClearRootMotion();
                return;
            }
            
            if (motor.GroundingStatus.IsStableOnGround)
            {
                return;
            }

            if (_forwardAxis > 0f)
            {
                // If we want to move, add an acceleration to the velocity
                Vector3 rootMotionTargetMovementVelocity = motor.CharacterForward * _forwardAxis * MaxAirMoveSpeed;
                Vector3 velocityDiff = Vector3.ProjectOnPlane(rootMotionTargetMovementVelocity - currentVelocity, CustomPhysics.Gravity);
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
            ClearRootMotion();
            _lookInputVector = Vector3.zero;
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

        private void ClearRootMotion()
        {
            MoveRootMotionVector = Vector3.zero;
            LookRootMotionQuaternion = Quaternion.identity;
        }
    }
}
