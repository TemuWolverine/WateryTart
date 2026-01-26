using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;
using WateryTart.Settings;

namespace WateryTart.ViewModels
{
    public class HomeViewModel : ReactiveObject, IViewModelBase
    {
        private readonly IMassWsClient _massClient;
        private readonly ISettings _settings;
        private readonly IScreen _screen;
        public ObservableCollection<Recommendation> Recommendations { get; set; }

        public ReactiveCommand<Item, Unit> ClickedCommand { get; }

        public HomeViewModel(IScreen screen, IMassWsClient massClient, ISettings settings)
        {
            Title = "Home";
            _massClient = massClient;
            _settings = settings;
            _screen = screen;

            Recommendations = new ObservableCollection<Recommendation>();
            _massClient.MusicRecommendations(RecommendationHandler);

            ClickedCommand = ReactiveCommand.Create<Item>(item =>
            {
                var i = item; //navigate to whatever

                switch (i.MediaType)
                {
                    case MediaType.Album:

                        var vm = WateryTart.App.Container.GetRequiredService<AlbumViewModel>();

                        vm.LoadFromId(item.ItemId, item.Provider);
                        screen.Router.Navigate.Execute(vm);
                        break;

                    case MediaType.Playlist:
                        var playlistViewModel = WateryTart.App.Container.GetRequiredService<PlaylistViewModel>();
                        playlistViewModel.LoadFromId(item.ItemId, item.Provider);
                        screen.Router.Navigate.Execute(playlistViewModel);
                        break;

                    case MediaType.Artist:
                        var artistViewModel = WateryTart.App.Container.GetRequiredService<ArtistViewModel>();
                        artistViewModel.LoadFromId(item.ItemId, item.Provider);
                        screen.Router.Navigate.Execute(artistViewModel);
                        break;

                    case MediaType.Genre:
                        break;

                    case MediaType.Radio: break;
                    case MediaType.Track: break;

                    case MediaType.Audiobook: break;
                    case MediaType.Folder:break;
                    case MediaType.Podcast: break;
                    case MediaType.PodcastEpisode: break;
                }
            });
        }

        public void RecommendationHandler(RecommendationResponse response)
        {
            var nonEmptyRecommendations = response
                .Result
                .Where(r => r.items.Any());

            foreach (var n in nonEmptyRecommendations)
            {
                if (n.items.Count > 4)
                    n.items = n.items.GetRange(0, 4);
                Recommendations.Add(n);
            }
        }

        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
        public string Title { get; set; }
    }
}