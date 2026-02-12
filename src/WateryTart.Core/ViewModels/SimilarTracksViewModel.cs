using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WateryTart.Core.Extensions;
using WateryTart.MusicAssistant;
using WateryTart.MusicAssistant.Models;
using WateryTart.MusicAssistant.WsExtensions;

namespace WateryTart.Core.ViewModels
{
    public class SimilarTracksViewModel : ReactiveObject, IViewModelBase
    {
        private readonly MusicAssistantClient _client;
        public string? UrlPathSegment { get; } = "SimilarTracks/ID";
        public IScreen HostScreen { get; }
        public string Title { get; } = string.Empty;
        public bool ShowMiniPlayer { get; } = true;
        public bool ShowNavigation { get; } = true;

        public ObservableCollection<IViewModelBase> Tracks { get; set; } = new ObservableCollection<IViewModelBase>();
        public SimilarTracksViewModel(IScreen screen, MusicAssistantClient client)
        {
            _client = client;
            HostScreen = screen;
        }

        public async Task LoadFromId(string id, string provider)
        {
            var results = await _client.WithWs().GetMusicSimilarTracksAsync(id, provider);
            foreach (var i in results.Result)
            {
               Tracks.Add(i.CreateViewModelForItem());
            }
        }

    }
}
