using System;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Enums;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Camera.Systems
{
    [Serializable]
    public class CameraTargetMoveSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CameraTargetComp, TransformComp>> _cameraTargetFilter = default;
        private readonly EcsFilterInject<Inc<CameraFollowMeTransformComp>> _cameraFollowMeFilter = default;

        private readonly EcsPoolInject<TransformComp> _transformPool = default;
        private readonly EcsPoolInject<CameraFollowMeTransformComp> _cameraFollowMeTransformPool = default;

        private Vector3 _cachedPosition;
        private int _highestPriority;

        public void Run(IEcsSystems systems)
        {
            _highestPriority = int.MinValue;
            
            if (_cameraTargetFilter.Value.GetEntitiesCount() <= 0 || _cameraFollowMeFilter.Value.GetEntitiesCount() <= 0)
            {
                return;
            }
            
            foreach (var cameraTargetEntity in _cameraTargetFilter.Value)
            {
                ref var transform = ref _transformPool.Value.Get(cameraTargetEntity);

                foreach (var cameraFollowMeEntity in _cameraFollowMeFilter.Value)
                {
                    ref var cameraFollowMeTransformComp = ref _cameraFollowMeTransformPool.Value.Get(cameraFollowMeEntity);

                    if (_highestPriority >= cameraFollowMeTransformComp.Priority)
                    {
                        continue;
                    }
                    
                    _highestPriority = cameraFollowMeTransformComp.Priority;
                    _cachedPosition = cameraFollowMeTransformComp.Transform.position;
                }
                
                transform.Value.position = _cachedPosition;
            }
        }
    }
}