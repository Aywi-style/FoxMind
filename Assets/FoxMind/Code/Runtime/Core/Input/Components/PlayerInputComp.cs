using Unity.Mathematics;

namespace FoxMind.Code.Runtime.Core.Input.Components
{
    public struct PlayerInputComp
    {
        public float2 Direction;
        public bool Jump;
        public bool Attack;
    }
}