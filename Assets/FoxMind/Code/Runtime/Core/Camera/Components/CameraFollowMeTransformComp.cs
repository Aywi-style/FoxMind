using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    /// <summary>
    /// Компонент вешается на отслеживаемую сущность
    /// </summary>
    [Serializable]
    public struct CameraFollowMeTransformComp : IEntityFeature<CameraFollowMeTransformComp>
    {
        public int Priority;
        public Transform Transform;
    }
}