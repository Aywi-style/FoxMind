using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    [Serializable]
    public struct VisualHolderComp : IEntityFeature<VisualHolderComp>
    {
        public GameObject Value;
    }
}