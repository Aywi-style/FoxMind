using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Battle.Statuses.Interfaces
{
    public interface IStatusComponent
    {
        
    }
    
    public interface IStatusComponent<T> : IEntityFeature<T>, IStatusComponent where T : struct
    {
        
    }
}