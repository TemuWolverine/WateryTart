using Avalonia;
using ReactiveUI.Avalonia;
using System;
using System.Reflection;
using Velopack;
using WateryTart.Core;
using WateryTart.Core.Playback;
using WateryTart.Platform.Windows.ViewModels;
using WateryTart.Platform.Windows.Views;

namespace WateryTart.Platform.Windows;
    
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
            ViewLocator.RegisterView<SimpleWasapiPlayerSettingsViewModel, SimpleWasapiPlayerSettingsView>();
            
            var x = new App(
            [
                new InstancePlatformSpecificRegistration<IPlayerFactory>(new WindowsAudioPlayerFactory()),
                new LambdaRegistration<SimpleWasapiPlayerSettingsViewModel>(
                    c => new SimpleWasapiPlayerSettingsViewModel(/* pass dependencies */)
                )
            ]);
            return x;
        })
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .LogToTrace();
}
