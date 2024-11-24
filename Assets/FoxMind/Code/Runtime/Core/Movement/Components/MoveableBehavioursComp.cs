using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using FoxMind.Code.Runtime.Core.Movement.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Components
{
    [Serializable]
    public struct MoveableBehavioursComp : IEntityFeature<MoveableBehavioursComp>
    {
        [SerializeReference] private List<IMovementBehaviour> _movementBehavioursOnlyInspector;
        [SerializeReference] [CanBeNull] public IJumpBehaviour JumpBehaviour;
        public Dictionary<Type, IMovementBehaviour> MovementBehaviours;

        public void SetComposeValues(ref MoveableBehavioursComp component)
        {
            component = this;
            component.MovementBehaviours = new Dictionary<Type, IMovementBehaviour>();
            foreach (var characterController in component._movementBehavioursOnlyInspector)
            {
                component.MovementBehaviours.Add(characterController.GetType(), characterController);
            }
        }
    }
}