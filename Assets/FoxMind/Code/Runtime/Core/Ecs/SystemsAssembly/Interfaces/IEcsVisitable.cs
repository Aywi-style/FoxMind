using System;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Enums;
using FoxMind.Code.Runtime.Plugins.Visitor;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces
{
    public interface IEcsVisitable : IVisitableItem<IEcsVisitor>, IEcsSystem
    {
        public bool IsEnabled { get; set; }
        public SystemUpdateType UpdateType { get; set; }
        
        void IVisitableItem<IEcsVisitor>.Accept(IEcsVisitor visitor)
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