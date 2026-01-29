using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;
using WateryTart.Services;

namespace WateryTart.ViewModels
{
    public partial class ArtistViewModel : ReactiveObject, IViewModelBase
    {
        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
        private readonly IMassWsClient _massClient;
        private readonly IPlayersService _playersService;
        public bool ShowMiniPlayer => true;
        public bool ShowNavigation => true;
        [Reactive] public partial string Title { get; set; }
        [Reactive] public partial Artist Artist { get; set; }
        [Reactive] public partial ObservableCollection<Album> Albums { get; set; } = new();

        public ObservableCollection<Item> Tracks { get; set; }

        public ArtistViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
        {
            _massClient = massClient;
            _playersService = playersService;
            HostScreen = screen;
            Title = "";
        }

        public void LoadFromId(string id, string provider)
        {
            Tracks = new ObservableCollection<Item>();

            LoadArtistAlbum(id, provider);
            LoadArtist(id, provider);
        }

        private async Task LoadArtist(string id, string provider)
        {
            var artistResponse = await _massClient.ArtistGetAsync(id, provider);

            Artist = artistResponse.Result;
            Title = Artist.Name;
        }

        private async Task LoadArtistAlbum(string id, string provider)
        {
            var albumArtistResponse = await _massClient.ArtistAlbumsAsync(id, provider);
            foreach (var r in albumArtistResponse.Result.OrderByDescending(a => a.Year).ThenBy(a => a.Name))
                Albums.Add(r);
        }
    }
}