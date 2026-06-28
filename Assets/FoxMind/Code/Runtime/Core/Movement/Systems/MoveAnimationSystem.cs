using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Movement.Components;
using FoxMind.Code.Runtime.Core.StandaloneComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.Systems
{
    /// <summary>
    /// Система сетит параметры из movement для анимации движения
    /// </summary>
    public class MoveAnimationSystem : BaseEcsVisitable, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MotionAnimationComp, MoveableComp, TransformComp>> _animableFilter = default;
        
        readonly EcsPoolInject<TransformComp> _transformPool = default;
        readonly EcsPoolInject<MotionAnimationComp> _motionAnimationPool = default;
        readonly EcsPoolInject<MoveableComp> _moveablePool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var movableEntity in _animableFilter.Value)
            {
                ref var motionAnimation = ref _motionAnimationPool.Value.Get(movableEntity);

                if (motionAnimation.MoveState == null)
                {
                    continue;
                }
                
                if (motionAnimation.MoveState.IsActive == false)
                {
                    continue;
                }
                
                ref var transform = ref _transformPool.Value.Get(movableEntity);
                ref var moveable = ref _moveablePool.Value.Get(movableEntity);

                /*float animationX = Vector3.Dot(transform.Value.right, moveable.NormalizedMoveDirection);
                float animationY = Vector3.Dot(transform.Value.forward, moveable.NormalizedMoveDirection);*/
                float currentSpeed = moveable.Motor.Velocity.magnitude; // Получаем величину текущей скорости
                float normalizedSpeed;
                float maxSpeed = moveable.CustomCharacterController.CurrentMovementBehaviour.GetMaxSpeed();
                
                if (maxSpeed <= 0) // Обработка деления на ноль
                {
                    normalizedSpeed = 0;
                }
                else
                {
                    normalizedSpeed = Mathf.Clamp01(currentSpeed / maxSpeed); // Нормализуем и ограничиваем в диапазоне [0, 1]
                }
                
                float animationX = Vector3.Dot(transform.Value.right, moveable.Motor.Velocity);
                float animationY = Vector3.Dot(transform.Value.forward, moveable.Motor.Velocity);

                motionAnimation.MoveState.Parameter = new Vector2(animationX, animationY) * normalizedSpeed;
            }
        }
    }
}