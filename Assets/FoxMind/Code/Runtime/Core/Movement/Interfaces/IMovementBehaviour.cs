using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Interfaces
{
    public interface IMovementBehaviour : ICharacterController
    {
        void Enter();
        void Exit();
        void Update();
        void Initialize(KinematicCharacterMotor motor);
        void SetMoveDirection(Vector3 normalizedMoveDirection);
        void SetLookDirection(Vector3 normalizedLookDirection);
        float GetMaxSpeed();
    }
}
