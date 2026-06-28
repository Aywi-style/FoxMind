using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    public class UnknownMovementBehaviour : IMovementBehaviour
    {
        private static UnknownMovementBehaviour _instance;
        
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            throw new System.NotImplementedException();
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            throw new System.NotImplementedException();
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            throw new System.NotImplementedException();
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            throw new System.NotImplementedException();
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(KinematicCharacterMotor motor)
        {
            throw new System.NotImplementedException();
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            throw new System.NotImplementedException();
        }

        public void SetLookDirection(Vector3 normalizedLookDirection)
        {
            throw new System.NotImplementedException();
        }

        public float GetMaxSpeed()
        {
            throw new System.NotImplementedException();
        }

        public static IMovementBehaviour Get()
        {
            _instance ??= new UnknownMovementBehaviour();

            return _instance;
        }
    }
}