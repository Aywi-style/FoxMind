using System;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Movement.MonoBehaviours
{
    /// <summary>
    /// Отвечает за мувмент главного героя
    /// </summary>
    public class CustomCharacterController : MonoBehaviour, ICharacterController
    {
        [field: SerializeField] public KinematicCharacterMotor Motor { private set; get; }

        [ShowInInspector] public IMovementBehaviour CurrentMovementBehaviour { private set; get; }
        [ShowInInspector] public IJumpBehaviour CurrentJumpBehaviour { private set; get; }
        
        [field: SerializeField] public bool RootMotion { set; get; } = false;

        private void Start()
        {
            Motor.CharacterController = this;
        }

        public void Initialize(IMovementBehaviour baseMovementBehaviour, IJumpBehaviour baseJumpBehaviour)
        {
            SetCurrentMovementBehaviour(baseMovementBehaviour);
            SetJumpBehaviour(baseJumpBehaviour);
        }
        
        public void SetCurrentMovementBehaviour(IMovementBehaviour newMovementBehaviour)
        {
            CurrentMovementBehaviour = newMovementBehaviour;
        }

        public void SetJumpBehaviour(IJumpBehaviour jumpBehaviour)
        {
            CurrentJumpBehaviour = jumpBehaviour;
        }
        
        private void Update()
        {
            CurrentMovementBehaviour?.Update();

            Debug.Log(Motor.Velocity);
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            CurrentMovementBehaviour?.SetMoveDirection(normalizedMoveDirection);
        }

        public void SetJumpRequest()
        {
            CurrentJumpBehaviour?.SetJumpRequest();
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            CurrentMovementBehaviour?.UpdateRotation(ref currentRotation, deltaTime);
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            CurrentMovementBehaviour?.UpdateVelocity(ref currentVelocity, deltaTime);
            
            CurrentJumpBehaviour?.UpdateVelocity(ref currentVelocity, deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            CurrentJumpBehaviour?.AfterCharacterUpdate(deltaTime);
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
            CurrentJumpBehaviour?.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
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