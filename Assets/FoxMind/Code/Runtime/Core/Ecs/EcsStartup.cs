using FoxMind.Code.Runtime.Core.Ecs.Aspects;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs
{
    public class EcsStartup : MonoBehaviour, IEcsVisitor
    {
        EcsWorld _world;
        IEcsSystems _systems;
        [SerializeReference] private BaseSystemAssembly[] _systemAssemblies;
        
        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            for (int i = 0; i < _systemAssemblies.Length; i++)
            {
                if (_systemAssemblies[i] == null)
                {
                    Debug.LogError($"System Assemblies with {i} index is null!");
                    
                    continue;
                }

                _systemAssemblies[i].Accept(this);
            }
            
            _systems
#if UNITY_EDITOR
            .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
            .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
#endif
            .Inject()    
            //.InjectAspect(new AspectTest())
            .Init();
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
            
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

        public void Visit(IEcsSystem item)
        {
            _systems.Add(item);
        }
    }
}