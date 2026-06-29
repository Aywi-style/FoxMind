using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Battle.Targeting.Systems;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Interfaces;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.SystemsAssembly
{
    /// <summary>
    /// Сборка ECS-систем таргетинга: регистрация состояния, валидация hard target,
    /// расчёт soft target, обработка hard target, manual aim и синхронизация UI.
    /// </summary>
    public class TargetingAssembly : BaseSystemAssembly
    {
        protected override void CreateSystems()
        {
            EcsVisitable = new List<IEcsVisitable>
            {
                new ValidateCurrentTargetSystem(),
                new SoftTargetingSystem(),
                new HardTargetingSelectionSystem(),
                new TargetingManualAimSystem(),
                new UpdateDirectionToTargetSystem(),
                new UpdateTargetingUiSystem(),
            };
        }
    }
}
