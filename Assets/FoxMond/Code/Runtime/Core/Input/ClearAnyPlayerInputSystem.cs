using Unity.Entities;

namespace Fearness.Code.Runtime.Core.Input
{
    [UpdateBefore(typeof(PlayerInputSystem))]
    public partial struct ClearAnyPlayerInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CPlayerInput>();
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var jumpSignal in SystemAPI.Query<EnabledRefRW<CJumpSignal>>().WithAll<CPlayerInput>())
            {
                jumpSignal.ValueRW = false;
            }
        }
    }
}