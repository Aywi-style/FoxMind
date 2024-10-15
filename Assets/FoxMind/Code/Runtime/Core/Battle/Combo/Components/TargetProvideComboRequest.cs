using FoxMind.Code.Runtime.Core.Battle.Combo.Configs;
using Leopotam.EcsLite;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Components
{
    public struct TargetProvideComboRequest
    {
        public EcsPackedEntity PackedEntity;
        public ComboConfig_v2 ComboConfig;
    }
}