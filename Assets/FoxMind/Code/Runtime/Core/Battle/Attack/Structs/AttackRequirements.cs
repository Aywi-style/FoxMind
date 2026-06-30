using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Input.Structs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Attack.Structs
{
    [Serializable]
    public struct AttackRequirements
    {
        [field: SerializeField] public ComboStanceCondition StanceCondition { set; get; }
        [field: SerializeField] public List<ComboInput> MovementSetup { set; get; }
        [field: SerializeField] public List<ComboInput> Activators { set; get; }
    }
}