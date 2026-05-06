using FoxMind.Code.Runtime.ProjectScope;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Camera
{
    public class CoreCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        public UnityEngine.Camera Camera => _camera;

        public void Awake()
        {
            R.CoreCamera = this;
        }
    }
}