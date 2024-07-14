using FoxMind.Code.Runtime.Core.Visitor;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces
{
    public interface IEcsVisitable : IVisitableItem<IEcsVisitor>, IEcsSystem
    {
        public bool IsEnabled { get; set; }
        
        void IVisitableItem<IEcsVisitor>.Accept(IEcsVisitor visitor)
        {
            if (IsEnabled == false)
            {
                return;
            }
            
            visitor.Visit(this);
        }
    }
}