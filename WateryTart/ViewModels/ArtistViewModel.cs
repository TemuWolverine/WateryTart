using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReactiveUI.SourceGenerators;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;
using WateryTart.Services;

namespace WateryTart.ViewModels
{
    public partial class ArtistViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
        private readonly IMassWsClient _massClient;
        private readonly IPlayersService _playersService;

        [Reactive] public partial Artist Artist { get; set; }

        public ObservableCollection<Item> Tracks { get; set; }

        public ArtistViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
        {
            _massClient = massClient;
            _playersService = playersService;
            HostScreen = screen;
        }


        public void LoadFromId(string id, string provider)
        {
            Tracks = new ObservableCollection<Item>();

            //_massClient.PlaylistTracksGet(id, provider, TrackListHandler);
            //_massClient.PlaylistGet(id, provider, PlaylistHandler);

        }

    }
}
