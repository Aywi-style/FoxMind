using FoxMind.Code.Runtime.Core.Ecs.Templates;

namespace FoxMind.Code.Runtime.Core.Input.Interfaces
{
    public interface IInputFeature
    {
        
    }
    
    public interface IInputFeature<T> : IEntityFeature<T>, IInputFeature where T : struct
    {
        
    }
}