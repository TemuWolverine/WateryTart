using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WateryTart.Core.Services;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
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
    [Reactive] public ObservableCollection<IViewModelBase> Items { get; set; }
    public AsyncRelayCommand LoadMoreCommand { get; }
    ICommand ILoadMoreListViewModel.LoadMoreCommand => LoadMoreCommand;
    public IViewModelBase? SelectedItem { get; set; }

    [Reactive] public partial bool UseWrapPanel { get; set; } = false;

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
                if (SelectedItem is INeedsLoadingViewModel)
                {
                    _ = ((INeedsLoadingViewModel)SelectedItem).LoadAsync();
                }
            }
        });

        LoadMoreCommand = new AsyncRelayCommand(LoadMoreAsync, () => !IsLoading && HasMoreItems);

        _ = LoadInitialAsync();
    }

    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

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
            _logger.LogError(ex, $"Error loading items: {ex.Message}");
            HasMoreItems = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadAlbumsAsync() =>
        await LoadItemsAsync<Album, AlbumsResponse>(
            () => _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: PageSize, offset: CurrentOffset),
            album => new AlbumViewModel(_client, HostScreen, _playersService, album));

    private async Task LoadArtistsAsync() =>
        await LoadItemsAsync<Artist, ArtistsResponse>(
            () => _client.WithWs().GetArtistsAsync(limit: PageSize, offset: CurrentOffset),
            artist => new ArtistViewModel(_client, HostScreen, _playersService!, artist));

    private async Task LoadInitialAsync()
    {
        CurrentOffset = 0;
        Items.Clear();
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
            foreach (var item in response.Result)
            {
                Items.Add(createViewModel(item));
            }

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
            () => _client.WithWs().GetPlaylistsAsync(limit: PageSize, offset: CurrentOffset),
            playlist => new PlaylistViewModel(_client, HostScreen, _playersService!, playlist));

    private async Task LoadTracksAsync() =>
        await LoadItemsAsync<Item, TracksResponse>(
            () => _client.WithWs().GetTracksAsync(limit: PageSize, offset: CurrentOffset),
            track => new TrackViewModel(_client, _playersService!, track));
}