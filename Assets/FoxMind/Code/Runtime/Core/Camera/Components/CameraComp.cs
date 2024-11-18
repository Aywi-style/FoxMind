using Cinemachine;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Camera.Components
{
    public struct CameraComp : IEntityFeature<CameraComp>
    {
        public UnityEngine.Camera Camera;
        public CinemachineVirtualCamera VirtualCamera;
    }
}