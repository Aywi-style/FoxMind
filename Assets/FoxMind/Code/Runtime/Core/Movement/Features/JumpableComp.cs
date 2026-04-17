using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Компонент отвечает за возможность сущности прыгать
    /// </summary>
    [Serializable]
    /*[Title("Feature: JumpableComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]*/
    public struct JumpableComp : IEntityFeature<JumpableComp>
    {
        
    }
}
