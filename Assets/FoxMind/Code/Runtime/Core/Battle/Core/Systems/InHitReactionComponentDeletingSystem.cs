using FoxMind.Code.Runtime.Core.Battle.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Systems
{
    /// <summary>
    /// Удаляет состояние hit reaction после окончания его временного окна.
    /// Для воздушного knockdown сначала ждёт приземления, и только потом запускает таймер реакции.
    /// </summary>
    public class InHitReactionComponentDeletingSystem : BaseEcsVisitable, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InHitReactionComp>> _inHitReactionFilter = default;

        private readonly EcsPoolInject<InHitReactionComp> _inHitReactionPool = default;
        private readonly EcsPoolInject<CharacterControllerComp> _characterControllerPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _inHitReactionFilter.Value)
            {
                ref var inHitReaction = ref _inHitReactionPool.Value.Get(entity);
                if (inHitReaction.WaitForGroundBeforeTimer)
                {
                    if (IsGrounded(entity) == false)
                    {
                        continue;
                    }

                    inHitReaction.WaitForGroundBeforeTimer = false;
                    inHitReaction.StartTime = Time.time;
                    inHitReaction.EndTime = Time.time + Mathf.Max(0.01f, inHitReaction.Duration);
                    continue;
                }

                if (Time.time < inHitReaction.EndTime)
                {
                    continue;
                }

                _inHitReactionPool.Value.Del(entity);
            }
        }

        private bool IsGrounded(int entity)
        {
            if (_characterControllerPool.Value.Has(entity) == false)
            {
                return true;
            }

            ref var characterController = ref _characterControllerPool.Value.Get(entity);
            return characterController.Value == null ||
                   characterController.Value.Motor == null ||
                   characterController.Value.Motor.GroundingStatus.IsStableOnGround;
        }
    }
}
