using Autofac;
using Avalonia;
using Microsoft.Extensions.Logging;
using ReactiveUI.Avalonia;
using System;
using Velopack;
using SillyMIDI.Core;
using SillyMIDI.Core.Playback;
using SillyMIDI.Core.Services;
using SillyMIDI.Platform.Windows.ViewModels;
using SillyMIDI.Platform.Windows.Views;

namespace SillyMIDI.Platform.Windows;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        VelopackApp.Build().Run();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>(() =>
        {
            // Register platform-specific views BEFORE creating App
            ViewLocator.RegisterView<PlaybackSettingsViewModel, PlaybackSettingsView>();

            var x = new App(
            [
                new LambdaRegistration<IPlayerFactory>(c => new WindowsAudioPlayerFactory(App.Container.Resolve<ILoggerFactory>())),
                new LambdaRegistration<IVolumeService>(c => new WindowsVolumeService(c.Resolve<PlayersService>())),
            ]);
            return x;
        })
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI(rxui =>
            {

            })
        .UseHarfBuzz()
            .LogToTrace();
}
