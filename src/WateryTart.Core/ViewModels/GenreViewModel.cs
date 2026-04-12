using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Threading.Tasks;
using WateryTart.Core.Services;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels
{
    public partial class GenreViewModel : ViewModelBase<GenreViewModel>, ILoadableViewModel<Genre>, ILoadAsync
    {

        [Reactive] public partial Genre Genre { get; set; }

        public GenreViewModel(MusicAssistantClient massClient, IScreen screen, PlayersService playersService, Genre? genre = null)
            : base(client: massClient, playersService: playersService)
        {
            HostScreen = screen;
            Genre = genre ?? new Genre();
        }

        public async Task LoadAsync()
        {
            if (Genre == null || Genre.ItemId == null || Genre.Provider == null)
                return;

            //Tracks = [];
            IsLoading = true;
            try
            {
                var playlistResponse = await _client.WithWs().GetGenreAsync(Genre.ItemId, Genre.Provider);
                if (playlistResponse?.Result != null)
                {
                    Genre = playlistResponse.Result;
                    Title = playlistResponse.Result.Name;
                }
            }
            catch (Exception ex)
            {
                App.Logger?.LogError(ex, $"Error loading playlists");
            }

            try
            {
                /*
                var tracksResponse = await _client.WithWs().GetPlaylistTracksAsync(Playlist.ItemId, Playlist.Provider);
                if (tracksResponse?.Result != null)
                {
                    foreach (var t in tracksResponse.Result)
                        Tracks?.Add(new TrackViewModel(_client, _playersService!, t));
                }

                var totalSeconds = Tracks?.Sum(t => t.Track?.Duration ?? 0) ?? 0;
                var ts = TimeSpan.FromSeconds((int)totalSeconds);
                RunningTime = $"{(int)ts.TotalHours}h {ts.Minutes}m";*/
            }
            catch (Exception ex)
            {
                App.Logger?.LogError(ex, $"Error loading genre tracks");
            }

            IsLoading = false;
        }

        public async Task SetAndLoadModel(Genre item)
        {
            Genre = item;
            await LoadAsync();
        }
    }
}