using System;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    /// <summary>
    /// Для корректной работы нужен RedirectRootMotionToSlayerJetCharacterController, чтобы последний перенаправлял информацию о движении из анимации сюда
    /// </summary>
    [Serializable]
    public class RootMotionStableMovementBehaviour : IMovementBehaviour, IRootMotion
    {
        [SerializeField] private KinematicCharacterMotor motor;
        
        [field: Header("Root Motion Stable Movement")]
        [field: SerializeField] public Vector3 MoveRootMotionVector { get; set; }
        [field: SerializeField] public Quaternion LookRootMotionQuaternion { get; set; }
        [SerializeField] private float OrientationSharpness = 10f;
        [SerializeField] private Vector3 _lookInputVector;

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
            if (_lookInputVector.sqrMagnitude > float.Epsilon && OrientationSharpness > 0f)
            {
                var smoothedLookDirection = Vector3.Slerp(
                    motor.CharacterForward,
                    _lookInputVector,
                    1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
                
                currentRotation = Quaternion.LookRotation(smoothedLookDirection, motor.CharacterUp);
            }
            else
            {
                currentRotation = LookRootMotionQuaternion * currentRotation;
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (deltaTime > 0)
            {
                currentVelocity = GetRootMotionVelocity(deltaTime);
            }
            else
            {
                // Prevent division by zero
                currentVelocity = Vector3.zero;
            }
            
            ClearRootMotion();
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            if (MoveRootMotionVector.y <= float.Epsilon)
            {
                return;
            }

            motor.ForceUnground(0.16f);
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

        private Vector3 GetRootMotionVelocity(float deltaTime)
        {
            return MoveRootMotionVector / deltaTime;
        }
    }
}
