using Unity.Entities;
using Unity.Mathematics;

namespace Fearness.Code.Runtime.Core.Input
{
    public struct CPlayerInput : IComponentData
    {
        public float2 Value;
    }
}