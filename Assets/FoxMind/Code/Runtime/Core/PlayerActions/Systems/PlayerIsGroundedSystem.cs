using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerIsGroundedSystem : BaseEcsVisitable, IEcsRunSystem
    {
        [SerializeField] private float _groundCheck;
        
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, PushBoxCapsuleComp>> _controlledFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        readonly EcsPoolInject<PushBoxCapsuleComp> _pushBoxPool = default;
        readonly EcsPoolInject<IsGroundedComp> _isGroundedPool = default;

        private RaycastHit[] _cachedResult = new RaycastHit[8];
        private int _cachedHitCounts;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var controlledEntity in _controlledFilter.Value)
            {
                ref var pushBox = ref _pushBoxPool.Value.Get(controlledEntity);
                ref var transform = ref _transformPool.Value.Get(controlledEntity);

                var startPoint = transform.Value.position + pushBox.Value.center;
                Debug.DrawRay(startPoint, Vector3.down * pushBox.Value.height, Color.red);
                _cachedHitCounts = Physics.RaycastNonAlloc(startPoint, Vector3.down, _cachedResult, pushBox.Value.height, LayerMask.GetMask("PushBox"));

                if (_cachedHitCounts > 0 && _isGroundedPool.Value.Has(controlledEntity) == false)
                {
                    _isGroundedPool.Value.Add(controlledEntity);
                }
                else if (_cachedHitCounts == 0 && _isGroundedPool.Value.Has(controlledEntity))
                {
                    _isGroundedPool.Value.Del(controlledEntity);
                }
            }
        }
    }
}