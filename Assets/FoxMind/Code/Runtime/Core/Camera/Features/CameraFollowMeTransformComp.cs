using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    /// <summary>
    /// Компонент вешается на отслеживаемую сущность
    /// </summary>
    [Serializable]
    /*[Title("Feature: CameraFollowMeTransformComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]*/
    public struct CameraFollowMeTransformComp : IEntityFeature<CameraFollowMeTransformComp>
    {
        public int Priority;
        public Transform Transform;
    }
}
