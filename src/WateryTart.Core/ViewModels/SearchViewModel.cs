using Autofac;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WateryTart.Core.Services;
using WateryTart.Core.Settings;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.Responses;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels;

public partial class SearchViewModel : ViewModelBase<SearchViewModel>
{
    private readonly CompositeDisposable _disposables;
    private readonly SourceList<MediaItemBase> _searchResults = new();
    private readonly CompositeDisposable _subscriptions = [];
    private ILoggerFactory _loggerFactory;
    private ReadOnlyObservableCollection<AlbumViewModel> searchAlbums;
    private ReadOnlyObservableCollection<ArtistViewModel> searchArtists;
    private ReadOnlyObservableCollection<AlbumViewModel> searchFullAlbums;
    private ReadOnlyObservableCollection<ArtistViewModel> searchFullArtists;
    private ReadOnlyObservableCollection<PlaylistViewModel> searchFullPlaylists;
    private ReadOnlyObservableCollection<TrackViewModel> searchFullTracks;
    private ReadOnlyObservableCollection<TrackViewModel> searchItem;
    private ReadOnlyObservableCollection<PlaylistViewModel> searchPlaylist;
    public RelayCommand ClearSearchResultsCommand { get; }
    public RelayCommand ExpandAlbumResultsCommand { get; }
    public RelayCommand ExpandArtistsResultsCommand { get; }
    public RelayCommand ExpandPlaylistResultsCommand { get; }
    public RelayCommand ExpandTracksResultsCommand { get; }
    [Reactive] public partial ObservableCollection<HeroSearchModel> HeroSearchItems { get; set; }
    [Reactive] public partial bool IsSearching { get; set; }
    [Reactive] public partial bool HasSearchResults { get; set; }
    public ObservableCollection<string> RecentSearchTerms { get; set; } = [];
    public RelayCommand<string> SearchRecentTermCommand { get; }
    public ReadOnlyObservableCollection<AlbumViewModel> SearchAlbums => searchAlbums;
    public ReadOnlyObservableCollection<ArtistViewModel> SearchArtists => searchArtists;
    public AsyncRelayCommand SearchCommand { get; }
    public ReadOnlyObservableCollection<AlbumViewModel> SearchFullAlbums => searchFullAlbums;
    public ReadOnlyObservableCollection<ArtistViewModel> SearchFullArtists => searchFullArtists;
    public ReadOnlyObservableCollection<PlaylistViewModel> SearchFullPlaylists => searchFullPlaylists;
    public ReadOnlyObservableCollection<TrackViewModel> SearchFullTracks => searchFullTracks;
    public ReadOnlyObservableCollection<TrackViewModel> SearchItem => searchItem;
    public ReadOnlyObservableCollection<PlaylistViewModel> SearchPlaylist => searchPlaylist;
    [Reactive] public partial string SearchTerm { get; set; }
    [Reactive] public partial int SelectedTabIndex { get; set; }
    [Reactive] public override partial string Title { get; set; } = "Search";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public SearchViewModel(MusicAssistantClient massclient, ISettings settings, PlayersService playersService, IScreen screen, ILoggerFactory loggerFactory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        : base(loggerFactory, massclient, playersService)
    {
        _settings = settings;
        HostScreen = screen;
        _disposables = [];
        _loggerFactory = loggerFactory;

        // Load recent search terms from settings
        RecentSearchTerms = new ObservableCollection<string>(_settings.RecentSearchTerms ?? []);

        HeroSearchItems =
        [
            new HeroSearchModel
            {
                Title = "Most Played Albums",
                Command = new RelayCommand(HeroSearchMostPlayedAlbums),
                BackgroundColor = new SolidColorBrush(Colors.Red),
            },
            new HeroSearchModel
            {
                Title = "Most Played Artists",
                Command = new RelayCommand(HeroSearchMostPlayedArtists),
                BackgroundColor = new SolidColorBrush(Colors.Orange),
            }
            ,
            new HeroSearchModel
            {
                Title = "Most Played Artists",
                Command = new RelayCommand(HeroSearchMostPlayedAlbums),
                BackgroundColor = new SolidColorBrush(Colors.Orange),
            }
        ];

        /* Switch tabs to show full results */
        ExpandArtistsResultsCommand = new RelayCommand(() => { SelectedTabIndex = 2; });
        ExpandAlbumResultsCommand = new RelayCommand(() => { SelectedTabIndex = 3; });
        ExpandTracksResultsCommand = new RelayCommand(() => { SelectedTabIndex = 1; });
        ExpandPlaylistResultsCommand = new RelayCommand(() => { SelectedTabIndex = 4; });

        // Command to clear all search results
        ClearSearchResultsCommand = new RelayCommand(() =>
        {
            SearchTerm = string.Empty;
            _searchResults.Clear();
            HasSearchResults = false;
            SelectedTabIndex = 0;
            RecentSearchTerms.Clear();
            _settings.RecentSearchTerms = [];
        });

        // Command to search for a recent term
        SearchRecentTermCommand = new RelayCommand<string>(term =>
        {
            if (!string.IsNullOrWhiteSpace(term))
            {
                SearchTerm = term;
                SelectedTabIndex = 0; // Switch to "All" tab
            }
        });

        // Create a debounced search command
        SearchCommand = new AsyncRelayCommand(async () =>
        {
            IsLoading = true;
            IsSearching = true;
            try
            {
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    SearchResponse results = await (_client.WithWs().SearchAsync(SearchTerm));
                    var searchResponse = results;
                    _searchResults.Clear();
#pragma warning disable CS8604 // Possible null reference argument.
                    _searchResults.AddRange(searchResponse.Result?.Albums);
                    _searchResults.AddRange(searchResponse.Result?.Artists);
                    _searchResults.AddRange(searchResponse.Result?.Playlists);
                    _searchResults.AddRange(searchResponse.Result?.Tracks);
#pragma warning restore CS8604 // Possible null reference argument.

                    HasSearchResults = _searchResults.Count > 0;

                    // Save to recent searches only after successful search
                    AddToRecentSearches(SearchTerm);
                }
                else
                {
                    _searchResults.Clear();
                    HasSearchResults = false;
                }
            }
            finally
            {
                IsSearching = false;
                IsLoading = false;
            }
        });

        // Debounce SearchTerm changes and trigger search after 1.5 seconds of inactivity
        this.WhenAnyValue(x => x.SearchTerm)
            .Throttle(TimeSpan.FromSeconds(0.5), RxSchedulers.MainThreadScheduler)
            .Select(_ => Unit.Default)
            .Subscribe(_ => SearchCommand.Execute(null))
            .DisposeWith(_disposables);

        /* Avalonia doesn't have CVS, so apply filtering to collections using Rx */
        WireArtists();
        WireAlbums();
        WireTracks();
        WirePlaylists();
    }

    public void Reap()
    {
        _subscriptions?.Dispose();
    }

    public async Task ReapAsync()
    {
        _subscriptions?.Dispose();
    }

    private void AddToRecentSearches(string term)
    {
        // Remove the term if it already exists
        if (RecentSearchTerms.Contains(term))
        {
            RecentSearchTerms.Remove(term);
        }

        // Add to the beginning of the list
        RecentSearchTerms.Insert(0, term);

        // Keep only the last 10 searches
        while (RecentSearchTerms.Count > 10)
        {
            RecentSearchTerms.RemoveAt(RecentSearchTerms.Count - 1);
        }

        // Save to settings
        _settings.RecentSearchTerms = RecentSearchTerms.ToList();
    }

    private void HeroSearchMostPlayedAlbums()
    {
        var mostplayedAlbums = new LoadMoreListViewModel<ArtistViewModel>(_client, HostScreen, _playersService!, _loggerFactory, "Most Played Albums", true);
        mostplayedAlbums.SetCustomDataSource<Album, AlbumsResponse>(
            async () =>
            {
                var albums = await _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: 50, order: MusicAssistant.Models.Enums.OrderBy.play_count_desc, offset: mostplayedAlbums.CurrentOffset);
                return albums;
            },
            a =>
            {
                var vm = App.Container.Resolve<AlbumViewModel>();
                vm.Album = a;
                return vm;
            });

        HostScreen.Router.Navigate.Execute(mostplayedAlbums);
    }

    private void HeroSearchMostPlayedArtists()
    {
        var mostPlayedArtists = new LoadMoreListViewModel<ArtistViewModel>(_client, HostScreen, _playersService!, _loggerFactory, "Most Played Albums", true);
        mostPlayedArtists.SetCustomDataSource<Artist, ArtistsResponse>(
            async () =>
            {
                var artist = await _client.WithWs().GetArtistsAsync(limit: 50, order: MusicAssistant.Models.Enums.OrderBy.play_count_desc, offset: mostPlayedArtists.CurrentOffset);
                return artist;
            },
            a =>
            {
                var vm = App.Container.Resolve<ArtistViewModel>();
                vm.Artist = a;
                return vm;
            });

        HostScreen.Router.Navigate.Execute(mostPlayedArtists);
    }
    private void WireAlbums()
    {
        /* Albums */
        var albumsObservable = _searchResults
            .Connect()
            .Filter(i => i is Album)
            .Transform(i => new AlbumViewModel(_client, HostScreen, _playersService!, (Album)i))
            .Publish();

        _subscriptions.Add(albumsObservable
            .Bind(out searchFullAlbums)
            .Subscribe());

        _subscriptions.Add(albumsObservable
            .Top(3)
            .Bind(out searchAlbums)
            .Subscribe());

        _subscriptions.Add(albumsObservable.Connect());
    }

    private void WireArtists()
    {
        /* Artists */
        var artistsObservable = _searchResults
            .Connect()
            .Filter(i => i is Artist)
            .Transform(i => new ArtistViewModel(_client, HostScreen, _playersService!, (Artist)i))
            .Publish();
        _subscriptions.Add(artistsObservable
            .Bind(out searchFullArtists)
            .Subscribe());
        _subscriptions.Add(artistsObservable
            .Top(3)
            .Bind(out searchArtists)
            .Subscribe());
        _subscriptions.Add(artistsObservable.Connect());
    }

    private void WirePlaylists()
    {
        /* Playlists */
        var playlistsObservable = _searchResults
            .Connect()
            .Filter(i => i is Playlist)
            .Transform(i => new PlaylistViewModel(_client, HostScreen, _playersService!, (Playlist)i))
            .Publish();

        _subscriptions.Add(playlistsObservable
            .Bind(out searchFullPlaylists)
            .Subscribe());

        _subscriptions.Add(playlistsObservable
            .Top(3)
            .Bind(out searchPlaylist)
            .Subscribe());

        _subscriptions.Add(playlistsObservable.Connect());
    }

    private void WireTracks()
    {
        /* Tracks */
        var tracksObservable = _searchResults
            .Connect()
            .Filter(i => i is Item)
            .Transform(i => new TrackViewModel(_client, _playersService!, (Item)i))
            .Publish();

        _subscriptions.Add(tracksObservable
            .Bind(out searchFullTracks)
            .Subscribe());

        _subscriptions.Add(tracksObservable
            .Top(3)
            .Bind(out searchItem)
            .Subscribe());

        _subscriptions.Add(tracksObservable.Connect());
    }
}