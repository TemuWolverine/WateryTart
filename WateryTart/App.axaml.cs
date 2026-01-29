using AsyncImageLoader.Loaders;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Config.Net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using WateryTart.MassClient;
using WateryTart.Services;
using WateryTart.Settings;
using WateryTart.ViewModels;
using WateryTart.ViewModels.Players;


namespace WateryTart;

public static class AntipatternExtensionsYesIKnowItsBad
{
    public static T GetRequiredService<T>(this IContainer c)
    {
        return c.Resolve<T>();
    }
}
public partial class App : Application
{
    private static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Library", "WateryTart");
    private static readonly Lazy<DiskCachedWebImageLoader> LazyImageLoader = new(() => new DiskCachedWebImageLoader(AppDataPath));
    private static string _cachedBaseUrl;
    private static IEnumerable<IReaper> _reapers;

    public static IContainer Container;
    public static string BaseUrl
    {
        get
        {
            if (string.IsNullOrEmpty(_cachedBaseUrl))
            {
                _cachedBaseUrl = Container.Resolve<ISettings>().Credentials.BaseUrl;
            }
            return _cachedBaseUrl;
        }
    }

    public static DiskCachedWebImageLoader ImageLoaderInstance => LazyImageLoader.Value;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var builder = new ContainerBuilder();

        //Services
        builder.RegisterType<MainWindowViewModel>().As<IScreen>().SingleInstance();
        builder.RegisterType<MassWsClient>().As<IMassWsClient>().SingleInstance();
        builder.RegisterType<PlayersService>().As<IPlayersService>().SingleInstance();

        //Settings
        var settings = new ConfigurationBuilder<ISettings>().UseJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Library", "WateryTart")).Build();
        builder.RegisterInstance<ISettings>(settings).SingleInstance();

        //View models that are singleton
        builder.RegisterType<SettingsViewModel>().SingleInstance();
        builder.RegisterType<PlayersViewModel>().SingleInstance();
        builder.RegisterType<MiniPlayerViewModel>().SingleInstance();
        builder.RegisterType<BigPlayerViewModel>().SingleInstance();
        builder.RegisterType<HomeViewModel>().SingleInstance();

        //Volume controllers
        builder.RegisterType<WindowsVolumeService>().AsImplementedInterfaces().SingleInstance();
#if ARMRELEASE
        builder.RegisterType<GpioVolumeService>().AsImplementedInterfaces().SingleInstance();
#endif

        //Transient viewmodels
        builder.RegisterType<AlbumsListViewModel>();
        builder.RegisterType<AlbumViewModel>();
        builder.RegisterType<LoginViewModel>();
        builder.RegisterType<PlaylistViewModel>();
        builder.RegisterType<ArtistViewModel>();
        builder.RegisterType<SearchViewModel>();
        builder.RegisterType<ArtistsViewModel>();
        builder.RegisterType<LibraryViewModel>();
        builder.RegisterType<RecommendationViewModel>();

        Container = builder.Build();

        // Cache BaseUrl immediately after container is built
        _cachedBaseUrl = Container.Resolve<ISettings>().Credentials.BaseUrl;
        
        // Cache reapers for shutdown
        _reapers = Container.Resolve<IEnumerable<IReaper>>();

        var vm = Container.Resolve<IScreen>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };

            //Shutdown
            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).ShutdownRequested += (s, e) =>
            {
                foreach (var reaper in _reapers)
                {
                    reaper.Reap();
                }
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}