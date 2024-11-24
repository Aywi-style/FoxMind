using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Interfaces
{
    public interface IJumpBehaviour
    {
        void Initialize(KinematicCharacterMotor motor);
        void SetJumpRequest();
        void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);
        void AfterCharacterUpdate(float deltaTime);

        void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport);
    }
}