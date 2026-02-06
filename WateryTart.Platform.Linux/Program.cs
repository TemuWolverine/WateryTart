using Avalonia;
using ReactiveUI.Avalonia;
using System;
using System.Reflection;
using Autofac;
using WateryTart.Core;
using WateryTart.Core.Playback;
using WateryTart.Core.Services;
using WateryTart.Core.Settings;
using WateryTart.Platform.Linux.ViewModels;
using WateryTart.Platform.Linux.Views;

namespace WateryTart.Platform.Linux;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>(() =>
        {
            ViewLocator.RegisterView<GpioVolumeSettingsViewModel, GpioVolumeSettingsView>();


            var x = new App(
                [
                    new InstancePlatformSpecificRegistration<IPlayerFactory>(new LinuxAudioPlayerFactory()),
                    new LambdaRegistration<GpioVolumeService>(c => new GpioVolumeService(c.Resolve<ISettings>(),c.Resolve<IPlayersService>())),
                    new LambdaRegistration<GpioVolumeSettingsViewModel>(c => new GpioVolumeSettingsViewModel()),
                ]);

            return x;
        })
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .WithDeveloperTools()
            .LogToTrace();
}