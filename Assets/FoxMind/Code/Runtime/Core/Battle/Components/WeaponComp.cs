using System;
using FoxMind.Code.Runtime.Core.Battle.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    [Serializable]
    public struct WeaponComp : IEntityFeature<WeaponComp>
    {
        public HitBoxMb HitBoxMb;
    }
}