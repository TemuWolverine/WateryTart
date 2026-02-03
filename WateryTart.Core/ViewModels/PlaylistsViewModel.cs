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

public partial class PlaylistsViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; } = "Playlists";
    public IScreen HostScreen { get; }
    private readonly IMassWsClient _massClient;
    private readonly IPlayersService _playersService;

    [Reactive] public partial string Title { get; set; }
    [Reactive] public partial ObservableCollection<PlaylistViewModel> Playlists { get; set; } = new();
    public ReactiveCommand<PlaylistViewModel, Unit> ClickedCommand { get; }
    public bool ShowMiniPlayer => true;
    public bool ShowNavigation => true;

    public PlaylistsViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
    {
        _massClient = massClient;
        _playersService = playersService;
        HostScreen = screen;
        Title = "Playlists";

        ClickedCommand = ReactiveCommand.Create<PlaylistViewModel>(item =>
        {
            item.LoadFromId(item.Playlist.ItemId, item.Playlist.Provider);
            screen.Router.Navigate.Execute(item);
        });

#pragma warning disable CS4014 // Fire-and-forget intentional - loads data asynchronously
        _ = LoadAsync();
#pragma warning restore CS4014
    }

    private async Task LoadAsync()
    {
        try
        {
            Playlists.Clear();
            var response = await _massClient.PlaylistsGetAsync();
            
            if (response?.Result != null)
            {
                foreach (var playlist in response.Result)
                {
                    Playlists.Add(new PlaylistViewModel(_massClient, HostScreen, _playersService, playlist));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading playlists: {ex.Message}");
        }
    }
}