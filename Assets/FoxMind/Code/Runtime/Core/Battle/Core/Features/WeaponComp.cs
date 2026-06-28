using System;
using FoxMind.Code.Runtime.Core.Battle.MonoBehaviours;
using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Core.Features
{
    [Serializable]
    /*[Title("Feature: WeaponComp")]
    [InfoBox("Designer-facing entity feature. Add via EntityBaker.Features or EntityTemplateConfig.")]*/
    public struct WeaponComp : IEntityFeature<WeaponComp>
    {
        public HitBoxMb HitBoxMb;
    }
}
