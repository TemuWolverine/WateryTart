using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI.SourceGenerators;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;
using WateryTart.Services;

namespace WateryTart.ViewModels
{
    public partial class PlaylistViewModel : ReactiveObject, IViewModelBase
    {
        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
        private readonly IMassWsClient _massClient;
        private readonly IPlayersService _playersService;

        [Reactive] public partial Playlist Playlist { get; set; }
        [Reactive] public partial string Title { get; set; }

        public ObservableCollection<Item> Tracks { get; set; }

        public PlaylistViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
        {
            _massClient = massClient;
            _playersService = playersService;
            HostScreen = screen;
            Title = "";
        }

        public void LoadFromId(string id, string provider)
        {
            Tracks = new ObservableCollection<Item>();

            _massClient.PlaylistTracksGet(id, provider, TrackListHandler);
            _massClient.PlaylistGet(id, provider, PlaylistHandler);

        }

        private void PlaylistHandler(PlaylistResponse response)
        {
            this.Playlist= response.Result;
            Title = Playlist.Name;
        }


        public void TrackListHandler(TracksResponse response)
        {
            foreach (var t in response.Result)
                Tracks.Add(t);

        }


        
    }
}
