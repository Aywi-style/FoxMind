using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    /// <summary>
    /// Компонент, отвечающий за то, что сущность является просто маркером для камеры
    /// </summary>
    [Serializable]
    public struct CameraTargetComp : IEntityFeature<CameraTargetComp>
    {
        
    }
}