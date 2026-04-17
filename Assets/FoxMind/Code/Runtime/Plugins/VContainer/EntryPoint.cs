using System;
using UnityEngine;
using VContainer.Unity;

namespace FoxMind.Code.Runtime.Plugins.VContainer
{
    public abstract class EntryPoint : MonoBehaviour, IStartable, IDisposable
    {
        public abstract void Start();

        public abstract void Dispose();
    }

    public abstract class EntryPoint<T> : EntryPoint where T : LifetimeScope
    {
        
    }
}