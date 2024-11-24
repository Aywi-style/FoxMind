using Animancer;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Animancer
{
    public class RedirectRootMotionToSlayerJetCharacterController : RedirectRootMotion<CustomCharacterController>
    {
        public override Vector3 Position
        {
            get
            {
                if (Target.CurrentMovementBehaviour is IRootMotion rootMotion)
                {
                    return rootMotion.MoveRootMotionVector;
                }
                return default;
            }
            set
            {
                if (Target.CurrentMovementBehaviour is IRootMotion rootMotion)
                {
                    rootMotion.MoveRootMotionVector += value;
                }
            }
        }

        public override Quaternion Rotation
        {
            get
            {
                if (Target.CurrentMovementBehaviour is IRootMotion rootMotion)
                {
                    return rootMotion.LookRootMotionQuaternion;
                }
                return default;
            }
            set
            {
                if (Target.CurrentMovementBehaviour is IRootMotion rootMotion)
                {
                    rootMotion.LookRootMotionQuaternion *= value;
                }
            }
        }
    }
}