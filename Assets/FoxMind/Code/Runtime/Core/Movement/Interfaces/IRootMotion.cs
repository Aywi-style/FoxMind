using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Interfaces
{
    /// <summary>
    /// Нужен для определения RootMotion контроллера, чтобы можно было передавать ему данные из анимации
    /// </summary>
    public interface IRootMotion
    {
        public Vector3 MoveRootMotionVector { set; get; }
        public Quaternion LookRootMotionQuaternion { set; get; }
    }
}