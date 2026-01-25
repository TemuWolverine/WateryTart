using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI.SourceGenerators;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;
using WateryTart.Services;

namespace WateryTart.ViewModels;

public partial class AlbumViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IMassWsClient _massClient;
    private readonly IPlayersService _playersService;
    private Album _album;
    public string? UrlPathSegment { get; } = "Album/ID";
    public IScreen HostScreen { get; }

    [Reactive]  public partial Album Album { get; set; }

    public ObservableCollection<Item> Tracks { get; set; }

    public AlbumViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
    {
        _massClient = massClient;
        _playersService = playersService;
        HostScreen = screen;
    }

    public void LoadFromId(string id, string provider)
    {
        Tracks = new ObservableCollection<Item>();

        _massClient.MusicAlbumTracks(id, provider, TrackListHandler);
        _massClient.MusicAlbumGet(id, provider, AlbumHandler);

    }

    public void Load(Album album)
    {
        Album = album;
        Tracks = new ObservableCollection<Item>();

        _massClient.MusicAlbumTracks(album.ItemId, TrackListHandler);
    }

    public void AlbumHandler(AlbumResponse response)
    {
        this.Album = response.Result;
    }

    public void TrackListHandler(TracksResponse response)
    {
        foreach (var t in response.Result)
            Tracks.Add(t);

        var x = Tracks.Last();
        _playersService.Play(x);
    }
}