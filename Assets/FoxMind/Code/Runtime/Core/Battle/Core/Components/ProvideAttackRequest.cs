
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    public struct ProvideAttackRequest
    {
        public EcsPackedEntity PackedEntity;
        public AttackConfig AttackConfig;
    }
}