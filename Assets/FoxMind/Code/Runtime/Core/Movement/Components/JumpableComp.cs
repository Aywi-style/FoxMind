using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Компонент отвечает за возможность сущности прыгать
    /// </summary>
    [Serializable]
    public struct JumpableComp : IEntityFeature<JumpableComp>
    {
        
    }
}