using System;
using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.SystemsAssembly.Interfaces;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.SystemsAssembly.Abstracts
{
    [Serializable]
    public abstract class BaseSystemAssembly : BaseEcsVisitable
    {
        [SerializeReference] protected List<IEcsVisitable> EcsVisitable;

        protected BaseSystemAssembly()
        {
            CreateSystems();
        }

        protected abstract void CreateSystems();

        public override void Accept(IEcsVisitor visitor)
        {
            if (IsEnabled == false)
            {
                return;
            }

            for (int i = 0; i < EcsVisitable.Count; i++)
            {
                if (EcsVisitable[i] == null)
                {
                    Debug.LogError($"Ecs Visitable with {i} index is null!");
                    
                    continue;
                }
                
                EcsVisitable[i].Accept(visitor);
            }
        }
    }
}