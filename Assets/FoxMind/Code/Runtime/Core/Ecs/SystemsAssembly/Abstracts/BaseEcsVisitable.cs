using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts
{
    public abstract class BaseEcsVisitable : IEcsVisitable
    {
        [field: SerializeField] public bool IsEnabled { get; set; } = true;

        public virtual void Accept(IEcsVisitor visitor)
        {
            if (IsEnabled == false)
            {
                return;
            }
            
            visitor.Visit(this);
        }
    }
}