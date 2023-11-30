using Fearness.Code.Runtime.Core.Input;
using Fearness.Code.Runtime.Core.Movable;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Fearness.Code.Runtime.Core.PlayerActions
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct PlayerMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            new PlayerMoveJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel(state.Dependency);
        }
    }
    
    [BurstCompile]
    public partial struct PlayerMoveJob : IJobEntity
    {
        public float DeltaTime;

        [BurstCompile]
        private void Execute(ref LocalTransform transform, in CPlayerInput playerInput, CMoveSpeed moveSpeed, CPlayerControlled playerControlled)
        {
            transform.Position.xz += playerInput.Value * moveSpeed.Value * DeltaTime;
            
            if (math.lengthsq(playerInput.Value) > float.Epsilon)
            {
                var forward = new float3(playerInput.Value.x, 0f, playerInput.Value.y);
                transform.Rotation = quaternion.LookRotation(forward, math.up());
            }
        }
    }
}