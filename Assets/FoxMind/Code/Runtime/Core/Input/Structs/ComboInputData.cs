using FoxMind.Code.Runtime.Core.Input.Enums;

namespace FoxMind.Code.Runtime.Core.Input.Structs
{
    public struct ComboInputData
    {
        public ComboInput InputData;
        public float Time;

        public bool IsMove()
        {
            return InputData.Type
                is ComboInputType.MoveForward
                or ComboInputType.MoveBackward
                or ComboInputType.MoveLeft
                or ComboInputType.MoveRight;
        }

        public bool IsComboActivator()
        {
            return InputData.Type
                is ComboInputType.MeleeAttack
                or ComboInputType.AltMeleeAttack
                or ComboInputType.RangeAttack
                or ComboInputType.AltRangeAttack;
        }
        
        public static ComboInputData Create(ComboInputType type, PressType pressType)
        {
            return new ComboInputData
            {
                InputData = new ComboInput
                {
                    Type = type,
                    PressType = pressType
                },
                Time = UnityEngine.Time.time
            };
        }

        public static ComboInputData Create(ComboInput comboInput)
        {
            return new ComboInputData
            {
                InputData = comboInput,
                Time = UnityEngine.Time.time
            };
        }
    }
}