using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Отвечает за связь систем с Kinematic ассетом. Внутри линк на CharacterController
    /// </summary>
    [Serializable]
    [Title("Feature: SlayerJetCharacterControllerComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct SlayerJetCharacterControllerComp : IEntityFeature<SlayerJetCharacterControllerComp>
    {
        public CustomCharacterController Value;
    }
}
