using System;
using FoxMind.Code.Runtime.Core.Input.Enums;

namespace FoxMind.Code.Runtime.Core.Input.Structs
{
    public struct ComboInput : IEquatable<ComboInput>
    {
        public ComboInputType Type;
        public PressType PressType;

        public bool Equals(ComboInput other)
        {
            return Type == other.Type && PressType == other.PressType;
        }

        public override bool Equals(object obj)
        {
            return obj is ComboInput other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, (int)PressType);
        }
    }
}