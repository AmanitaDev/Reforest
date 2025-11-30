using DayNightCycle;
using GameTemplate.Scripts.Systems.Audio;
using GameTemplate.Scripts.Systems.CursorLock;
using GameTemplate.Scripts.UI.Game;
using GameTemplate.Scripts.UI.Game.EscapeMenu;
using VContainer;
using VContainer.Unity;

namespace GameTemplate.Scripts.Scopes
{
    public class GameScope : LifetimeScope
    {
        public TimeSettingsSO timeSettingsSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<CursorLocker>(Lifetime.Scoped);
            
            builder.RegisterComponentInHierarchy<GameCanvasView>();
            builder.RegisterEntryPoint<GameCanvasController>();
            
            builder.RegisterEntryPoint<EscapeCanvasController>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<EscapeCanvasView>();

            builder.RegisterInstance(timeSettingsSo);
            builder.Register<TimeService>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces().As<ITickable>();
            builder.RegisterComponentInHierarchy<TimeView>();
            
            // Register the MenuMusicStarter as an entry point
            // VContainer will instantiate this class and call its Start() method.
            builder.RegisterEntryPoint<GameMusicStarter>();
        }
    }
}