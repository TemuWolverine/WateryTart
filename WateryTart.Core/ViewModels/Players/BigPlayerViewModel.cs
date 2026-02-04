using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive;
using System.Reactive.Linq;
using WateryTart.Core.Services;

namespace WateryTart.Core.ViewModels.Players;

public partial class BigPlayerViewModel : ReactiveObject, IViewModelBase
{
    private readonly IPlayersService _playersService;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public bool ShowMiniPlayer => false;
    public bool ShowNavigation => false;
    public string Title { get; set; } = "";

    private double _cachedImageWidth = 300;
    private double _cachedImageHeight = 300;
    [Reactive] public partial bool IsSmallDisplay { get; set; }
    public double CachedImageWidth
    {
        get => _cachedImageWidth;
        set => this.RaiseAndSetIfChanged(ref _cachedImageWidth, value);
    }

    public double CachedImageHeight
    {
        get => _cachedImageHeight;
        set => this.RaiseAndSetIfChanged(ref _cachedImageHeight, value);
    }

    public IPlayersService PlayersService => _playersService;

    public ReactiveCommand<Unit, Unit> PlayerPlayPauseCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayerPreviousCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayNextCommand { get; }

    public BigPlayerViewModel(IPlayersService playersService, IScreen screen)
    {
        _playersService = playersService;
        HostScreen = screen;

        // Create a CanExecute observable that checks if a player is selected
        var canExecute = this.WhenAnyValue(x => x._playersService.SelectedPlayer)
            .Select(player => player != null)
            .ObserveOn(RxApp.MainThreadScheduler)
            .DistinctUntilChanged();

        PlayerPlayPauseCommand = ReactiveCommand.CreateFromTask(
            () => _playersService.PlayerPlayPause(_playersService.SelectedPlayer));

        PlayerPreviousCommand = ReactiveCommand.CreateFromTask(
            () => _playersService.PlayerPrevious(_playersService.SelectedPlayer),
            canExecute);

        PlayNextCommand = ReactiveCommand.CreateFromTask(
            () => _playersService.PlayerNext(_playersService.SelectedPlayer),
            canExecute);
    }

    public void UpdateCachedDimensions(double width, double height)
    {
        if (width > 0 && height > 0)
        {
            CachedImageWidth = width;
            CachedImageHeight = height;
        }
    }
}