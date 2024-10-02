using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    public struct HitBoxComp : IEntityFeature<HitBoxComp>
    {
        public Collider Collider;
    }
}