using FoxMind.Code.Runtime.Core.Ecs.Templates;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.StandaloneComponents
{
    public struct TransformComp : IEntityFeature<TransformComp>
    {
        public Transform Value;
        
        /*public void SetComposeValues(ref TransformComp component)
        {
            component.Value = Value;
        }*/
    }
}