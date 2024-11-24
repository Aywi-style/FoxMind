using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Interfaces
{
    public interface IMovementBehaviour : ICharacterController
    {
        void Update();
        void Initialize(KinematicCharacterMotor motor);
        void SetMoveDirection(Vector3 normalizedMoveDirection);
    }
}