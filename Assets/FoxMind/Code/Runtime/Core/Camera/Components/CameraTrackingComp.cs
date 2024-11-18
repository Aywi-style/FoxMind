using System;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    /// <summary>
    /// Компонент, помогающий определить за какой сущностью камера будет следить. Выбирается сущность с наивысшим приоритетом
    /// </summary>
    [Serializable]
    public struct CameraTrackingComp : IEntityFeature<CameraTrackingComp>
    {
        public int Priority;
    }
}