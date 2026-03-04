using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WateryTart.Core.Services;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.Models.Enums;
using WateryTart.MusicAssistant.Responses;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels;

public partial class LoadMoreListViewModel<T> : ViewModelBase<LoadMoreListViewModel<T>>, ILoadMoreListViewModel
{
    private const int PageSize = 50;
    [Reactive] public partial int CurrentOffset { get; set; } = 0;
    public RelayCommand GoToItem { get; }
    [Reactive] public partial bool HasMoreItems { get; set; } = true;
    [Reactive] public override partial bool IsLoading { get; set; }
    [Reactive] public partial ObservableCollection<IViewModelBase> Items { get; set; }

    public IEnumerable<OrderBy> SortingOptions { get; } = Enum.GetValues<OrderBy>().Skip(1);
    [Reactive] public partial OrderBy SelectedSortingOption { get; set; } = OrderBy.name;
    public AsyncRelayCommand LoadMoreCommand { get; }
    ICommand ILoadMoreListViewModel.LoadMoreCommand => LoadMoreCommand;
    public IViewModelBase? SelectedItem { get; set; }

    [Reactive] public partial bool UseWrapPanel { get; set; } = false;

    private Func<Task>? _customLoadFunc;

    /// <summary>
    /// Sets a custom data source for loading items. This allows using the LoadMoreListViewModel
    /// with data sources not predefined in the type switch.
    /// </summary>
    /// <typeparam name="TItem">The type of item returned by the API (e.g., Album, Track).</typeparam>
    /// <typeparam name="TResponse">The response type that wraps the list of items.</typeparam>
    /// <param name="fetchFunc">Function that fetches data from the API, using CurrentOffset and PageSize.</param>
    /// <param name="createViewModel">Function that converts an API item to a ViewModel.</param>
    public void SetCustomDataSource<TItem, TResponse>(
            Func<Task<TResponse>> fetchFunc,
            Func<TItem, IViewModelBase> createViewModel)
            where TResponse : ResponseBase<List<TItem>>
    {
        _customLoadFunc = async () => await LoadItemsAsync(fetchFunc, createViewModel);
    }

    public LoadMoreListViewModel(MusicAssistantClient massClient, IScreen screen, PlayersService playersService, ILoggerFactory loggerFactory, string title, bool useWrapPanel = false)
        : base(loggerFactory, massClient, playersService, screen)
    {
        Title = title;
        Items = [];
        UseWrapPanel = useWrapPanel;

        GoToItem = new RelayCommand(() =>
        {
            if (SelectedItem != null)
            {
                screen.Router.Navigate.Execute(SelectedItem);
                if (SelectedItem is ILoadAsync async)
                {
                    _ = async.LoadAsync();
                }
            }
        });

        // React to sorting changes with debounce to avoid rapid repeated loads
        this.WhenAnyValue(x => x.SelectedSortingOption)
            .Throttle(TimeSpan.FromMilliseconds(150)) // Debounce rapid changes
            .ObserveOn(AvaloniaScheduler.Instance)
            .SelectMany(_ => Observable.FromAsync(() => LoadInitialAsync()))
            .Subscribe();

        LoadMoreCommand = new AsyncRelayCommand(LoadMoreAsync, () => !IsLoading && HasMoreItems);
    }

    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            // Check for custom data source first
            if (_customLoadFunc != null)
            {
                await _customLoadFunc();
                return;
            }

            var t = typeof(T);
            switch (t)
            {
                case var _ when t == typeof(ArtistViewModel):
                    await LoadArtistsAsync();
                    break;

                case var _ when t == typeof(TrackViewModel):
                    await LoadTracksAsync();
                    break;

                case var _ when t == typeof(AlbumViewModel):
                    await LoadAlbumsAsync();
                    break;

                case var _ when t == typeof(PlaylistViewModel):
                    await LoadPlaylistsAsync();
                    break;

                // These are the likely candidates for future expansion, but not implemented yet:
                // genres, (relying on MA implementation?)
                // podcasts,
                // radios,
                // audiobooks

                default:
                    // fallback
                    break;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading items:{message}", ex.Message);
            HasMoreItems = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadAlbumsAsync() =>
        await LoadItemsAsync<Album, AlbumsResponse>(
            () => _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: PageSize, offset: CurrentOffset, order: SelectedSortingOption),
            album => new AlbumViewModel(_client, HostScreen, _playersService, album));

    private async Task LoadArtistsAsync() =>
        await LoadItemsAsync<Artist, ArtistsResponse>(
            () => _client.WithWs().GetArtistsAsync(limit: PageSize, offset: CurrentOffset, order: SelectedSortingOption),
            artist => new ArtistViewModel(_client, HostScreen, _playersService!, artist));

    private async Task LoadInitialAsync()
    {
        CurrentOffset = 0;
        
        // Clear on UI thread
        await Observable.Start(() => Items.Clear(), AvaloniaScheduler.Instance);
        
        await LoadAsync();
    }

    private async Task LoadItemsAsync<TItem, TResponse>(
        Func<Task<TResponse>> fetchFunc,
        Func<TItem, IViewModelBase> createViewModel)
        where TResponse : ResponseBase<List<TItem>>
    {
        var response = await fetchFunc();

        if (response?.Result != null)
        {
            // Build list off UI thread
            var newItems = response.Result
                .Select(item => createViewModel(item))
                .ToList();

            // Single UI thread update - convert observable to task
            await Observable.Start(() =>
            {
                foreach (var item in newItems)
                {
                    Items.Add(item);
                }
            }, AvaloniaScheduler.Instance);

            HasMoreItems = response.Result.Count == PageSize;

            if (HasMoreItems)
            {
                CurrentOffset += PageSize;
            }
        }
        else
        {
            HasMoreItems = false;
        }
    }

    private async Task LoadMoreAsync()
    {
        await LoadAsync();
    }

    private async Task LoadPlaylistsAsync() =>
        await LoadItemsAsync<Playlist, PlaylistsResponse>(
            () => _client.WithWs().GetPlaylistsAsync(limit: PageSize, offset: CurrentOffset, orderby: SelectedSortingOption),
            playlist => new PlaylistViewModel(_client, HostScreen, _playersService!, playlist));

    private async Task LoadTracksAsync() =>
        await LoadItemsAsync<Item, TracksResponse>(
            () => _client.WithWs().GetTracksAsync(limit: PageSize, offset: CurrentOffset, order: SelectedSortingOption),
            track => new TrackViewModel(_client, _playersService!, track));
}