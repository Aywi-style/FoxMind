using System;
using FoxMind.Code.Runtime.Core.Input.Structs;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Collections
{
    [Serializable]
    public class RingBuffer_ComboInputAction : RingBuffer<ComboInputData>
    {
        public RingBuffer_ComboInputAction(int capacity) : base(capacity)
        {
        }

        public override void Push(ComboInputData item)
        {
            if (item.IsComboActivator())
            {
                Debug.Log($"{item.Time} : {item.InputData.Type} | {item.InputData.PressType}");
            }
            
            base.Push(item);
        }
    }
}