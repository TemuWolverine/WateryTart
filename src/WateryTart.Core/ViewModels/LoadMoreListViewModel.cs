using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WateryTart.Core.Services;
using WateryTart.MusicAssistant;
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
                screen.Router.Navigate.Execute(SelectedItem);
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

                default:
                    // fallback
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error loading albums: {ex.Message}");
            HasMoreItems = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadAlbumsAsync()
    {
        var response = await _client.WithWs().GetMusicAlbumsLibraryItemsAsync(limit: PageSize, offset: CurrentOffset);

        if (response?.Result != null)
        {
            foreach (var album in response.Result)
            {
                Items.Add(new AlbumViewModel(_client, HostScreen, _playersService, album));
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

    private async Task LoadArtistsAsync()
    {
        var response = await _client.WithWs().GetArtistsAsync(limit: PageSize, offset: CurrentOffset);

        if (response?.Result != null)
        {
            foreach (var artist in response.Result)
            {
                Items.Add(new ArtistViewModel(_client, HostScreen, _playersService!, artist));
            }

            HasMoreItems = response.Result.Count == PageSize;

            if (HasMoreItems)
                CurrentOffset += PageSize;
        }
        else
        {
            HasMoreItems = false;
        }
    }

    private async Task LoadInitialAsync()
    {
        CurrentOffset = 0;
        Items.Clear();
        await LoadAsync();
    }

    private async Task LoadMoreAsync()
    {
        await LoadAsync();
    }

    private async Task LoadTracksAsync()
    {
        var response = await _client.WithWs().GetTracksAsync(limit: PageSize, offset: CurrentOffset);

        if (response?.Result != null)
        {
            foreach (var track in response.Result)
            {
                Items.Add(new TrackViewModel(_client, _playersService!, track));
            }

            HasMoreItems = response.Result.Count == PageSize;

            if (HasMoreItems)
                CurrentOffset += PageSize;
        }
        else
        {
            HasMoreItems = false;
        }
    }
}