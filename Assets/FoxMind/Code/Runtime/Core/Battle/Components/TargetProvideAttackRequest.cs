
using FoxMind.Code.Runtime.Core.Battle.Attack.Configs;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Components
{
    public struct TargetProvideAttackRequest
    {
        public EcsPackedEntity PackedEntity;
        public AttackConfig AttackConfig;
    }
}