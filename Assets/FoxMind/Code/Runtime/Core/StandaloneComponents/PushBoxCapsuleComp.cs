using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    public struct PushBoxCapsuleComp : IEntityFeature<PushBoxCapsuleComp>
    {
        public CapsuleCollider Value;
    }
}