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
        
        [field: SerializeField] public Vector3 MoveRootMotionVector { get; set; }
        [field: SerializeField] public Quaternion LookRootMotionQuaternion { get; set; }

        public void Initialize(KinematicCharacterMotor motor)
        {
            this.motor = motor;
        }

        public void Update()
        {
            
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            
        }
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            currentRotation = LookRootMotionQuaternion * currentRotation;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (deltaTime > 0)
            {
                // The final velocity is the velocity from root motion reoriented on the ground plane
                currentVelocity = MoveRootMotionVector / deltaTime;
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, motor.GroundingStatus.GroundNormal) *
                                  currentVelocity.magnitude;
            }
            else
            {
                // Prevent division by zero
                currentVelocity = Vector3.zero;
            }
            
            MoveRootMotionVector = Vector3.zero;
            LookRootMotionQuaternion = Quaternion.identity;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            MoveRootMotionVector = Vector3.zero;
            LookRootMotionQuaternion = Quaternion.identity;
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