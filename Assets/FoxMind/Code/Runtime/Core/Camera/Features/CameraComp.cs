using Cinemachine;
using FoxMind.Code.Runtime.Core.Ecs.Templates;
using Sirenix.OdinInspector;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    [Title("Feature: CameraComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]
    public struct CameraComp : IEntityFeature<CameraComp>
    {
        public UnityEngine.Camera Camera;
        public CinemachineVirtualCamera VirtualCamera;
    }
}
