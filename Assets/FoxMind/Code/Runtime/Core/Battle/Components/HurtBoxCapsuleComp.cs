using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    public struct HurtBoxCapsuleComp : IEntityFeature<HurtBoxCapsuleComp>
    {
        public CapsuleCollider Value;
    }
}