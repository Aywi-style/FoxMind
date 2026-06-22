using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Collections;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Input.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;

namespace FoxMind.Code.Runtime.Core.Input.Components
{
    /// <summary>
    /// Глобальное ECS-состояние базового input asset и последнего активного типа управления.
    /// </summary>
    public struct BaseInputControlsComp : IEntityFeature<BaseInputControlsComp>
    {
        public BaseControls Value;
        public InputControlType ActiveControlType;

        public RingBuffer_ComboInputAction BufferComboInputHistory;
        public ComboInputType CurrentComboMoveInput;
    }
}
