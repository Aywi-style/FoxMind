using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FoxMind.Code.Runtime.Plugins.VContainer
{
    public abstract class CustomLifetimeScope<T> : LifetimeScope where T : CustomLifetimeScope<T>
    {
        [SerializeField] private EntryPoint _entryPoint;
        
        protected override void Configure(IContainerBuilder builder)
        {
            CustomPreConfigure(builder);
            
            CustomPostConfigure(builder);

            if (_entryPoint == null)
            {
                Debug.LogError($"Энтри поинт для {typeof(T).Name} не был установлен!");
                return;
            }
            
            
            
            builder.RegisterComponent(_entryPoint);
        }

        protected virtual void CustomPreConfigure(IContainerBuilder builder)
        {
            
        }

        protected virtual void CustomPostConfigure(IContainerBuilder builder)
        {
            
        }
    }
}