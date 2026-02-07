using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WateryTart.Core.Services;

namespace WateryTart.Core.ViewModels.Players;

public partial class BigPlayerViewModel : ReactiveObject, IViewModelBase
{
    private readonly IPlayersService _playersService;
    public string? UrlPathSegment { get; } = "BigPlayer";
    public required IScreen HostScreen { get; set; }
    public bool ShowMiniPlayer => false;
    public bool ShowNavigation => false;
    public string Title { get; set; } = "";

    [Reactive] public partial bool IsSmallDisplay { get; set; }
    public double CachedImageWidth
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public double CachedImageHeight
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public IPlayersService PlayersService => _playersService;

    public ICommand PlayerNextCommand { get; set; }
    public ICommand PlayerPlayPauseCommand { get; set; }
    public ICommand PlayPreviousCommand { get; set; }

    public BigPlayerViewModel(IPlayersService playersService, IScreen screen)
    {
        PlayPreviousCommand = new RelayCommand(() => PlayersService.PlayerPrevious());
        PlayerNextCommand = new RelayCommand(() => PlayersService.PlayerNext());
        PlayerPlayPauseCommand = new RelayCommand(() => PlayersService.PlayerPlayPause());
        _playersService = playersService;
        HostScreen = screen;

        // Create a CanExecute observable that checks if a player is selected
        var canExecute = this.WhenAnyValue(x => x._playersService.SelectedPlayer)
            .Select(player => player != null)
            .ObserveOn(RxApp.MainThreadScheduler)
            .DistinctUntilChanged();
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