using FoxMind.Code.Runtime.Plugins.VContainer;
using UnityEngine;

namespace FoxMind.Code.Runtime.ProjectScope
{
    public class ProjectEntryPoint : EntryPoint<ProjectLifetimeScope>
    {
        public override void Start()
        {
            UnityEngine.Application.targetFrameRate = 60;
            
            Debug.Log("START");
        }

        public override void Dispose()
        {
            Debug.Log("Dispose");
        }
    }
}