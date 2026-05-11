using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using Sirenix.OdinInspector;
using UnityEngine.Scripting.APIUpdating;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Feature-компонент со ссылкой на CustomCharacterController сущности.
    /// Используется персонажем игрока и будущими врагами, которые двигаются через KinematicCharacterController.
    /// </summary>
    [Serializable]
    [MovedFrom(true, "FoxMind.Code.Runtime.Core.Movement.Components", null, "SlayerJetCharacterControllerComp")]
    /*[Title("Feature: CharacterControllerComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]*/
    public struct CharacterControllerComp : IEntityFeature<CharacterControllerComp>
    {
        public CustomCharacterController Value;
    }
}
