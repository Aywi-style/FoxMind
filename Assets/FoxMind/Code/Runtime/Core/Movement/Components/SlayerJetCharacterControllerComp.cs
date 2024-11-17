using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.MonoBehaviours;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    /// <summary>
    /// Отвечает за связь систем с Kinematic ассетом. Внутри линк на CharacterController
    /// </summary>
    [Serializable]
    public struct SlayerJetCharacterControllerComp : IEntityFeature<SlayerJetCharacterControllerComp>
    {
        public SlayerJetCharacterController Value;
    }
}