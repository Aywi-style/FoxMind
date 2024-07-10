using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Moving.Components
{
    public struct JumpableComp : IEntityFeature<JumpableComp>
    {
        public bool IsJumping;
        public bool IsGrounded;
        public float JumpImpulse;
    }
}