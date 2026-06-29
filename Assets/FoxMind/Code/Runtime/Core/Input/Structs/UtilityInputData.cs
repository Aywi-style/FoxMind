using FoxMind.Code.Runtime.Core.Input.Enums;

namespace FoxMind.Code.Runtime.Core.Input.Structs
{
    public struct UtilityInputData
    {
        public UtilityInputType Type;
        public PressType PressType;
        public float Time;
        
        
        
        public static UtilityInputData Create(UtilityInputType type, PressType pressType)
        {
            return new UtilityInputData
            {
                Type = type,
                PressType = pressType,
                Time = UnityEngine.Time.time
            };
        }
    }
}