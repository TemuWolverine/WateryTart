using Autofac;
using Avalonia;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Reflection;
using System.Threading;
using Velopack;
using Racket.Core;
using Racket.Core.Playback;
using Racket.Core.Services;
using Racket.Core.Settings;
using Racket.Platform.Linux.ViewModels;
using Racket.Platform.Linux.Views;

namespace Racket.Platform.Linux;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        VelopackApp.Build().Run();
        var builder = BuildAvaloniaApp();
        if (args.Contains("--drm"))
        {
            SilenceConsole();
            // By default, Avalonia will try to detect output card automatically.
            // But you can specify one, for example "/dev/dri/card1".
            return builder.StartLinuxDrm(args: args, card: null, scaling: 1.0);
        }

        return builder.StartWithClassicDesktopLifetime(args);
    }

    private static void SilenceConsole()
    {
        new Thread(() =>
        {
            Console.CursorVisible = false;
            while (true)
                Console.ReadKey(true);
        })
        { IsBackground = true }.Start();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>(() =>
        {
            Core.ViewLocator.RegisterView<GpioVolumeSettingsViewModel, GpioVolumeSettingsView>();


            var x = new App(
                [
                    new InstancePlatformSpecificRegistration<IPlayerFactory>(new LinuxAudioPlayerFactory()),
#if LINUX_ARM64         
                    new LambdaRegistration<GpioVolumeService>(c => new GpioVolumeService(c.Resolve<ISettings>(),c.Resolve<PlayersService>())),
                    new LambdaRegistration<IHaveSettings>(c =>
                        new GpioVolumeSettingsViewModel(
                            c.Resolve<ISettings>(),
                            c.Resolve<IScreen>(),
                            c.Resolve<GpioVolumeService>())),
#endif
                ]);

            return x;
        })
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI(rxui =>
            {
                // Optional: add custom registration here via rxui.WithRegistration(...)
            })
            .WithDeveloperTools()
            .LogToTrace();
}