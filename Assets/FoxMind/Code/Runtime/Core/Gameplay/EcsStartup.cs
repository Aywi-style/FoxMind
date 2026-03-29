using System;
using FoxMind.Code.Runtime.Core.Animations.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Battle.Combo.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Battle.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Camera.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Ecs.EcsAspects;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;
using FoxMind.Code.Runtime.Core.Input.SystemsAssembly;
using FoxMind.Code.Runtime.Core.InputTracking.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Movement.SystemsAssembly;
using FoxMind.Code.Runtime.Core.PlayerActions.SystemsAssembly;
using FoxMind.Code.Runtime.Core.Scenes.Init.SystemsAssembly;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Gameplay
{
    public class EcsStartup : MonoBehaviour, IEcsVisitor
    {
        EcsWorld _world;
#if UNITY_EDITOR
        IEcsSystems _editorSystems;
#endif
        IEcsSystems _updateSystems;
        IEcsSystems _lateUpdateSystems;
        IEcsSystems _fixedUpdateSystems;
        private readonly BaseSystemAssembly[] _systemAssemblies = new BaseSystemAssembly[]
        {
            new ScenesInitAssembly(),
            new CameraAssembly(),
            new InputAssembly(),
            new InputTrackingAssembly(),
            new ComboAssembly(),
            new BattleAssembly(),
            new AnimationsAssembly(),
            new PlayerActionAssembly(),
            new MovementAssembly()
        };
        
        private void Start()
        {
            _world = new EcsWorld();
            _updateSystems = new EcsSystems(_world);
            _lateUpdateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);
            
#if UNITY_EDITOR
            // Создаем отдельную группу для отладочных систем.
            _editorSystems = new EcsSystems (_world);
            _editorSystems
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Init ();
#endif

            for (int i = 0; i < _systemAssemblies.Length; i++)
            {
                if (_systemAssemblies[i] == null)
                {
                    Debug.LogError($"System Assemblies with {i} index is null!");
                    
                    continue;
                }

                _systemAssemblies[i].Accept(this);
            }

            InitSystem(_updateSystems, "Update");
            InitSystem(_lateUpdateSystems, "Late Update");
            InitSystem(_fixedUpdateSystems, "Fixed Update");
        }

        private void InitSystem(IEcsSystems system, string systemName)
        {
            system
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem(systemName))
#endif
                .Inject()    
                .InjectAspect(new AspectTest.Aspects.AspectTest())
                .Init();
        }
        
        private void Update()
        {
            _updateSystems?.Run();
#if UNITY_EDITOR
            // Выполняем обновление состояния отладочных систем. 
            _editorSystems?.Run ();
#endif
        }

        private void LateUpdate()
        {
            _lateUpdateSystems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystems?.Run();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            // Выполняем очистку отладочных систем.
            if (_editorSystems != null) {
                _editorSystems.Destroy ();
                _editorSystems = null;
            }
#endif
            
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }
            
            if (_lateUpdateSystems != null)
            {
                _lateUpdateSystems.Destroy();
                _lateUpdateSystems = null;
            }
            
            if (_fixedUpdateSystems != null)
            {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }
            
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }

        public void UpdateVisit(IEcsSystem item)
        {
            _updateSystems.Add(item);
        }

        public void LateUpdateVisit(IEcsSystem item)
        {
            _lateUpdateSystems.Add(item);
        }

        public void FixedUpdateVisit(IEcsSystem item)
        {
            _fixedUpdateSystems.Add(item);
        }
    }
}
