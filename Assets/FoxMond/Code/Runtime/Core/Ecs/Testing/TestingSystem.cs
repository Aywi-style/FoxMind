using Fearness.Code.Runtime.Core.Ecs.Combat;
using Unity.Entities;
using UnityEngine;

namespace Fearness.Code.Testing
{
    public partial struct TestingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<HealthComponent>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            Debug.Log("Работает");
        }
    }
}