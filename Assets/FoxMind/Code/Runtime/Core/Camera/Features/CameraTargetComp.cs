using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    /// <summary>
    /// Компонент, отвечающий за то, что сущность является просто маркером для камеры
    /// </summary>
    [Serializable]
    [Title("Feature: CameraTargetComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct CameraTargetComp : IEntityFeature<CameraTargetComp>
    {
        
    }
}
