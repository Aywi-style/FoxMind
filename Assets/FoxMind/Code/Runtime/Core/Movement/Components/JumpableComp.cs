using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    [Serializable]
    public struct JumpableComp : IEntityFeature<JumpableComp>
    {
        public float Impulse;
        public float CooldownMax;
        public float CooldownCurrent;
    }
}