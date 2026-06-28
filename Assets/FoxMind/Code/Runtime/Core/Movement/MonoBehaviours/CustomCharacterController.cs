using System;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours;
using FoxMind.Code.Runtime.Core.Stats.Features;
using UnityEngine;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Movement.MonoBehaviours
{
    /// <summary>
    /// Отвечает за мувмент персонажа
    /// </summary>
    public class CustomCharacterController : MonoBehaviour, ICharacterController
    {
        [field: SerializeField] public KinematicCharacterMotor Motor { private set; get; }
        
        [SerializeField] private LayerMask _ignoredMotorCollisionLayers;

        [field: Sirenix.OdinInspector.ReadOnly, ShowInInspector] public IMovementBehaviour CurrentMovementBehaviour { private set; get; }
        [field: Sirenix.OdinInspector.ReadOnly, ShowInInspector] public IJumpBehaviour CurrentJumpBehaviour { private set; get; }
        [field: Sirenix.OdinInspector.ReadOnly, ShowInInspector] public DashBehaviour DashBehaviour { private set; get; }

        private void Start()
        {
            Motor.CharacterController = this;
        }
        
        public void SetCurrentMovementBehaviour(IMovementBehaviour newMovementBehaviour)
        {
            if (CurrentMovementBehaviour == newMovementBehaviour)
            {
                return;
            }

            CurrentMovementBehaviour?.Exit();
            CurrentMovementBehaviour = newMovementBehaviour;
            CurrentMovementBehaviour?.Enter();
        }

        public void SetJumpBehaviour(IJumpBehaviour jumpBehaviour)
        {
            CurrentJumpBehaviour = jumpBehaviour;
        }

        public void SetDashBehaviour(DashBehaviour dashBehaviour)
        {
            DashBehaviour = dashBehaviour;
        }
        
        private void Update()
        {
            CurrentMovementBehaviour?.Update();
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            CurrentMovementBehaviour?.SetMoveDirection(normalizedMoveDirection);
        }

        public void SetLookDirection(Vector3 normalizedLookDirection)
        {
            CurrentMovementBehaviour?.SetLookDirection(normalizedLookDirection);
        }

        public void SetJumpRequest()
        {
            CurrentJumpBehaviour?.SetJumpRequest();
        }

        public void SetDashRequest(Vector3 dashDirection, UnitStatsComp unitStats)
        {
            DashBehaviour?.SetDashRequest(dashDirection, unitStats);
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
            CurrentMovementBehaviour?.BeforeCharacterUpdate(deltaTime);
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            CurrentMovementBehaviour?.PostGroundingUpdate(deltaTime);
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            CurrentMovementBehaviour?.AfterCharacterUpdate(deltaTime);
            CurrentJumpBehaviour?.AfterCharacterUpdate(deltaTime);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return (_ignoredMotorCollisionLayers.value & (1 << coll.gameObject.layer)) == 0;
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
