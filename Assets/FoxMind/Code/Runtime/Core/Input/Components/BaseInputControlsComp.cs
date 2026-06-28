using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Collections;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Input.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;
using Unity.Mathematics;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Input.Components
{
    /// <summary>
    /// Глобальное ECS-состояние базового input asset и последнего активного типа управления.
    /// </summary>
    public struct BaseInputControlsComp : IEntityFeature<BaseInputControlsComp>
    {
        public BaseControls Value;
        public InputControlType ActiveControlType;

        public Vector2 InputMoveDirection;
        
        // public RingBuffer<>
        
        public RingBuffer_ComboInputAction BufferComboInputHistory;
        public ComboInputType CurrentComboMoveInput;
    }
}
