using Animancer;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Animancer
{
    public class RedirectRootMotionToSlayerJetCharacterController : RedirectRootMotion<SlayerJetCharacterController>
    {
        public override Vector3 Position
        {
            get => Target.MoveRootMotionVector;
            set => Target.MoveRootMotionVector += value;
        }

        public override Quaternion Rotation
        {
            get => Target.LookRootMotionQuaternion;
            set => Target.LookRootMotionQuaternion *= value;
        }
    }
}