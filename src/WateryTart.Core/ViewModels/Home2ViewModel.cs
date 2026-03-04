using Autofac;
using Avalonia.Controls;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WateryTart.Core.Services;
using WateryTart.Core.Settings;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.Responses;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels;

public partial class Home2ViewModel : ViewModelBase<Home2ViewModel>
{
    private readonly ILoggerFactory _loggerFactory;
    [Reactive] public partial ObservableCollection<AlbumViewModel> DiscoverAlbums { get; set; }
    public ICommand DiscoverAlbumsCommand { get; set; }
    [Reactive] public partial ObservableCollection<ArtistViewModel> DiscoverArtists { get; set; }
    public ICommand DiscoverArtistsCommand { get; set; }
    [Reactive] public override partial bool IsLoading { get; set; }
    public ICommand RecentlyPlayedTracksCommand { get; set; }
    [Reactive] public partial ObservableCollection<TrackViewModel> RecentTracks { get; set; }
    public override string Title => "Home";

    public Home2ViewModel(
        IScreen screen,
        MusicAssistantClient maClient,
        ISettings settings,
        PlayersService playersService,
        ILoggerFactory loggerFactory) : base(loggerFactory, maClient)
    {
        _settings = settings;
        _playersService = playersService;
        _loggerFactory = loggerFactory;
        HostScreen = screen;

        DiscoverArtists = [];
        DiscoverAlbums = [];
        RecentTracks = [];

        //Set all commands
        DiscoverArtistsCommand = new RelayCommand(DiscoverArtistsClicked);
        DiscoverAlbumsCommand = new RelayCommand(DiscoverAlbumsClicked);
        RecentlyPlayedTracksCommand = new RelayCommand(RecentlyPlayedClicked);

        _ = LoadDataAsync();
    }

    private void DiscoverArtistsClicked()
    {
        var artistView = new LoadMoreListViewModel<ArtistViewModel>(_client, HostScreen, _playersService!, _loggerFactory, "Discover Artists", true);
        artistView.SetCustomDataSource<Artist, ArtistsResponse>(
            async () =>
            {
                var artists = await _client.WithWs().GetArtistsAsync(limit: 50, order_by: "random", album_artists_only: true);
                return artists;
            },
            a =>
            {
                var vm = App.Container.Resolve<ArtistViewModel>();
                vm.Artist = a;
                return vm;
            });

        HostScreen.Router.Navigate.Execute(artistView);
    }

    private void DiscoverAlbumsClicked()
    {
        var artistView = new LoadMoreListViewModel<AlbumViewModel>(_client, HostScreen, _playersService!, _loggerFactory, "Discover Albums", true);
        artistView.SetCustomDataSource<Album, AlbumsResponse>(
            async () =>
            {
                var albums = await _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: 50, order_by: "random");
                return albums;
            },
            a =>
            {
                var vm = App.Container.Resolve<AlbumViewModel>();
                vm.Album = a;
                return vm;
            });
        HostScreen.Router.Navigate.Execute(artistView);
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;

            var tasks = new List<Task>
            {
                Task.Run(async () =>
                {
                    if (RecentTracks.Count > 0) //Solves weird condition where this randomly gets hit twice.
                        return;
                    //Recent Tracks - api call
                    _logger.LogInformation("Fetching recent tracks...");
                    var recent = await _client.WithWs().GetRecentlyPlayedItemsAsync(limit: 5);
                    foreach (var r in recent.Result!)
                    {
                        if (RecentTracks.Any(t => t.Track.ItemId == r.ItemId)) // make sure no duplicates
                            continue;
                        var track = App.Container.Resolve<TrackViewModel>();
                        track.SetAndLoadModel(r);
                        RecentTracks.Add(track);
                    }
                }),

                Task.Run(async () =>
                {
                    //Discover Albums
                    //TODO: These should be cached for 12 hours?
                    var albums = await _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: 10, order_by: "random");

                    foreach (var a in albums.Result!)
                    {
                        var album = App.Container.Resolve<AlbumViewModel>();
                        album.Album = a;

                        DiscoverAlbums.Add(album);
                    }

                    _logger.LogInformation("Fetching discover albums...");
                }),

                Task.Run(async () =>
                {
                    //Discover Artists
                    //TODO: These should be cached for 12 hours?
                    var artists = await _client.WithWs().GetArtistsAsync(limit: 10, order_by: "random", album_artists_only: true);

                    foreach (var a in artists.Result!)
                    {
                        var artist = App.Container.Resolve<ArtistViewModel>();
                        artist.Artist = a;

                        DiscoverArtists.Add(artist);
                    }

                    _logger.LogInformation("Fetching discover artists...");
                })
            };

            //run all tasks simultaneously and wait for them to complete
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading Home2ViewModel");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void RecentlyPlayedClicked()
    {
        var recentVm = new LoadMoreListViewModel<TrackViewModel>(_client, HostScreen, _playersService!, _loggerFactory, "Recently Played Tracks", false);
        recentVm.SetCustomDataSource<Item, TracksResponse>(
            async () =>
            {
                var recent = await _client.WithWs().GetRecentlyPlayedItemsAsync(limit: 50/*, offset: recentVm.CurrentOffset*/);
                return recent;
            },
            t =>
            {
                var track = App.Container.Resolve<TrackViewModel>();
                track.SetAndLoadModel(t);
                return track;
            });

        HostScreen.Router.Navigate.Execute(recentVm);
    }
}