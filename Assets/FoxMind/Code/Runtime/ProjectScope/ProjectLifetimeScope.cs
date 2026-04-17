using CodiceApp;
using FoxMind.Code.Runtime.Plugins.VContainer;
using FoxMind.Code.Runtime.ProjectScope.Application.Core.Models;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FoxMind.Code.Runtime.ProjectScope
{
    public class ProjectLifetimeScope : CustomLifetimeScope<ProjectLifetimeScope>
    {
        /*[SerializeField] private LogConfig _logConfig;
        [SerializeField] private ProjectCanvas _projectCanvas;
        [SerializeField] private AssemblyCardsViewConfig _assemblyCardsViewConfig;
        [SerializeField] private EmojiAssemblyConfig _emojiAssemblyConfig;
        [SerializeField] private SceneConfig _sceneConfig;
        [SerializeField] private ApplicationConfig _applicationConfig;
        [SerializeField] private UiConfig _uiConfig;
        [SerializeField] private AlertConfig _alertConfig;
        [SerializeField] private ChipsConfig _chipsConfig;
        [SerializeField] private AvatarsAssemblyConfig _avatarsAssemblyConfig;*/

        // [Header("Audio Objects")]
        // [SerializeField] private AudioConfig _audioConfig;

        protected override void CustomPreConfigure(IContainerBuilder builder)
        {
            /*builder.RegisterComponent(_logConfig);
            builder.RegisterComponent(_sceneConfig);
            builder.RegisterComponent(_applicationConfig);
            builder.RegisterComponent(_uiConfig);
            builder.RegisterComponent(_alertConfig);
            builder.RegisterComponent(_projectCanvas);
            builder.RegisterComponent(_assemblyCardsViewConfig);
            builder.RegisterComponent(_emojiAssemblyConfig);
            builder.RegisterComponent(_chipsConfig);
            builder.RegisterComponent(_avatarsAssemblyConfig);*/

            /*builder.Register<LoggerSetuper>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LogWriteService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();*/
            builder.Register<ApplicationModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            // Settings
            builder.Register<SettingsModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}