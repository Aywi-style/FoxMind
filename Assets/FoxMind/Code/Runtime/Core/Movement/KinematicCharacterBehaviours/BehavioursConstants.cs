using System;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    public static class BehavioursConstants
    {
        public static readonly Type RootMotionStable = typeof(RootMotionStableMovementBehaviour);
        public static readonly Type Stable = typeof(StableMovementBehaviour);
        public static readonly Type RootMotionAir = typeof(RootMotionAirMovementBehaviour);
        public static readonly Type Air = typeof(AirMovementBehaviour);
        public static readonly Type Jump = typeof(JumpBehaviour);
    }
}