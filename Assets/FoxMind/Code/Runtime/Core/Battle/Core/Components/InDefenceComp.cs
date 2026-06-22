using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    public struct InDefenceComp
    {
        public bool IsDodge;
        public bool IsBlock => IsDodge == false;
        public Vector2 Direction;
        public float StartTime;
        public float EndTime;
    }
}