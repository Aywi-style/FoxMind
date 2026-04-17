using FoxMind.Code.Runtime.ProjectScope.Application.Core.Enums;
using R3;

namespace FoxMind.Code.Runtime.ProjectScope.Application.Core.Models
{
    public class ApplicationModel
    {
        private readonly ReactiveProperty<ApplicationState> _applicationState;
        public ReadOnlyReactiveProperty<ApplicationState> State { private set; get; }
        
        public ApplicationModel()
        {
            _applicationState = new ReactiveProperty<ApplicationState>();
            State = _applicationState.ToReadOnlyReactiveProperty();
        }
    }
}