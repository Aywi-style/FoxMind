using FoxMind.Code.Runtime.Core.Battle.Targeting.Components;
using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Camera.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.PlayerActions.Systems
{
    public class PlayerLookDirectionSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        
        readonly EcsFilterInject<Inc<InputDirectionComp>> _inputDirectionFilter = default;
        readonly EcsFilterInject<Inc<CameraComp, TransformComp>> _cameraFilter = default;
        readonly EcsFilterInject<Inc<PlayerControlledComp, TransformComp, MoveableComp>> _controlledTransformFilter = default;

        readonly EcsPoolInject<InputDirectionComp> _inputDirectionPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        readonly EcsPoolInject<TargetingComp> _targetingPool = default;
        readonly EcsPoolInject<TargetFindableComp> _targetFindablePool = default;
        readonly EcsPoolInject<InAttackComp> _inAttackPool = default;
        
        public void Run(IEcsSystems systems)
        {
            Vector3 cameraForward = new Vector3();
            Vector3 cameraRight = new Vector3();
            
            if (_cameraFilter.Value.GetEntitiesCount() > 0)
            {
                foreach (var cameraEntity in _cameraFilter.Value)
                {
                    ref var cameraTransform = ref _transformPool.Value.Get(cameraEntity);
                    cameraForward = cameraTransform.Value.forward;
                    cameraRight = cameraTransform.Value.right;
                    
                    break;
                }
            }
            
            foreach (var inputEntity in _inputDirectionFilter.Value)
            {
                ref var input = ref _inputDirectionPool.Value.Get(inputEntity);
                
                foreach (var controlledEntity in _controlledTransformFilter.Value)
                {
                    ref var transform = ref _transformPool.Value.Get(controlledEntity);
                    ref var moveable = ref _moveablePool.Value.Get(controlledEntity);

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    
                    cameraForward = cameraForward.sqrMagnitude > float.Epsilon ? cameraForward.normalized : Vector3.forward;
                    cameraRight = cameraRight.sqrMagnitude > float.Epsilon ? cameraRight.normalized : Vector3.right;
                    
                    var moveDirection = (cameraForward * input.Direction.y) + (cameraRight * input.Direction.x);
                    var lookDirection = GetLookDirection(controlledEntity, ref transform, moveDirection);

                    moveable.NormalizedLookDirection = lookDirection.sqrMagnitude > float.Epsilon
                        ? lookDirection.normalized
                        : Vector3.zero;
                }
            }
        }

        private Vector3 GetLookDirection(int controlledEntity, ref TransformComp transform, Vector3 moveDirection)
        {
            if (_targetingPool.Value.Has(controlledEntity))
            {
                ref var targeting = ref _targetingPool.Value.Get(controlledEntity);
                
                if (TryGetPreHitAttackTargetLookDirection(controlledEntity, ref transform, ref targeting, out var attackTargetDirection))
                {
                    return attackTargetDirection;
                }

                if (_inAttackPool.Value.Has(controlledEntity))
                {
                    return Vector3.zero;
                }
                
                if (targeting.IsManualAiming && targeting.ManualAimDirection.sqrMagnitude > float.Epsilon)
                {
                    return targeting.ManualAimDirection;
                }

                if (targeting.HasHardTarget
                    && targeting.HardTarget.Unpack(_world.Value, out var targetEntity)
                    && _transformPool.Value.Has(targetEntity))
                {
                    var targetPosition = GetTargetPosition(targetEntity);
                    var direction = targetPosition - transform.Value.position;
                    direction.y = 0f;
                    return direction;
                }
            }

            return moveDirection;
        }

        private bool TryGetPreHitAttackTargetLookDirection(int controlledEntity, ref TransformComp transform, ref TargetingComp targeting, out Vector3 direction)
        {
            direction = Vector3.zero;
            
            if (_inAttackPool.Value.Has(controlledEntity) == false)
            {
                return false;
            }

            ref var inAttack = ref _inAttackPool.Value.Get(controlledEntity);
            if (inAttack.AttackConfig == null)
            {
                return false;
            }

            var animationLength = inAttack.AnimationDuration;
            if (animationLength <= 0f)
            {
                return false;
            }

            var normalizedTime = (Time.time - inAttack.Start) / animationLength;
            var hitWindowStart = Mathf.Min(inAttack.AttackConfig.HitWindow.x, inAttack.AttackConfig.HitWindow.y);
            if (normalizedTime >= hitWindowStart)
            {
                return false;
            }

            if (targeting.IsManualAiming && targeting.ManualAimDirection.sqrMagnitude > float.Epsilon)
            {
                direction = targeting.ManualAimDirection;
                return true;
            }

            return TryGetTargetDirection(ref transform, targeting.HasHardTarget, targeting.HardTarget, out direction)
                   || TryGetTargetDirection(ref transform, targeting.HasSoftTarget, targeting.SoftTarget, out direction);
        }

        private bool TryGetTargetDirection(ref TransformComp transform, bool hasTarget, Leopotam.EcsLite.EcsPackedEntity target, out Vector3 direction)
        {
            direction = Vector3.zero;
            
            if (hasTarget == false || target.Unpack(_world.Value, out var targetEntity) == false || _transformPool.Value.Has(targetEntity) == false)
            {
                return false;
            }

            var targetPosition = GetTargetPosition(targetEntity);
            direction = targetPosition - transform.Value.position;
            direction.y = 0f;
            
            return direction.sqrMagnitude > float.Epsilon;
        }

        private Vector3 GetTargetPosition(int targetEntity)
        {
            if (_targetFindablePool.Value.Has(targetEntity))
            {
                ref var findable = ref _targetFindablePool.Value.Get(targetEntity);
                if (findable.TargetPoint != null)
                {
                    return findable.TargetPoint.position;
                }
            }

            return _transformPool.Value.Get(targetEntity).Value.position;
        }
    }
}
