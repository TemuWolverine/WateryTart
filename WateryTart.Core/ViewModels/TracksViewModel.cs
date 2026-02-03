using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using WateryTart.Core.Services;
using WateryTart.Service.MassClient;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.ViewModels;

public partial class TracksViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; } = "Tracks";
    public IScreen HostScreen { get; }
    private readonly IMassWsClient _massClient;
    private readonly IPlayersService _playersService;

    [Reactive] public partial string Title { get; set; }
    [Reactive] public partial ObservableCollection<TrackViewModel> Tracks { get; set; } = new();
    public ReactiveCommand<TrackViewModel, Unit> ClickedCommand { get; }
    public bool ShowMiniPlayer => true;
    public bool ShowNavigation => true;

    public TracksViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
    {
        _massClient = massClient;
        _playersService = playersService;
        HostScreen = screen;
        Title = "Tracks";

        ClickedCommand = ReactiveCommand.Create<TrackViewModel>(item =>
        {
            // Play the track or show context menu
            MessageBus.Current.SendMessage(MenuHelper.BuildStandardPopup(_playersService, item.Track));
        });

#pragma warning disable CS4014 // Fire-and-forget intentional - loads data asynchronously
        _ = LoadAsync();
#pragma warning restore CS4014
    }

    private async Task LoadAsync()
    {
        try
        {
            Tracks.Clear();
            var response = await _massClient.TracksGetAsync();
            
            if (response?.Result != null)
            {
                foreach (var track in response.Result)
                {
                    Tracks.Add(new TrackViewModel(_massClient, HostScreen, _playersService, track));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading tracks: {ex.Message}");
        }
    }
}