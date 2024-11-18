using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Enums;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts
{
    public abstract class BaseEcsVisitable : IEcsVisitable
    {
        [field: SerializeField] public bool IsEnabled { get; set; } = true;
        [field: SerializeField] public SystemUpdateType UpdateType { get; set; } = SystemUpdateType.Update;

        public virtual void Accept(IEcsVisitor visitor)
        {
            if (IsEnabled == false)
            {
                return;
            }
            
            switch (UpdateType)
            {
                case SystemUpdateType.Update:
                    visitor.UpdateVisit(this);
                    break;
                case SystemUpdateType.FixedUpdate:
                    visitor.FixedUpdateVisit(this);
                    break;
                case SystemUpdateType.LateUpdate:
                    visitor.LateUpdateVisit(this);
                    break;
                default:
                    visitor.UpdateVisit(this);
                    break;
            }
        }
    }
}