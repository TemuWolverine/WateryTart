using Autofac;
using CommunityToolkit.Mvvm.Input;
using IconPacks.Avalonia.Material;
using ReactiveUI;
using ReactiveUI.Avalonia;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SillyMIDI.Core.Services;
using SillyMIDI.Core.ViewModels.Menus;
using SillyMIDI.Core.ViewModels.Popups;
using SillyMIDI.MusicAssistant;
using SillyMIDI.MusicAssistant.Models.Enums;
using System.Reactive.Disposables;
using SillyMIDI.MusicAssistant.Models;
using SillyMIDI.Core.ViewModels.Players;
using SillyMIDI.MusicAssistant.WsExtensions;

namespace SillyMIDI.Core.ViewModels
{
    public partial class PlayerViewModel : ViewModelBase<PlayerViewModel>, IDisposable
    {
        private readonly CompositeDisposable _disposables = [];
        private readonly Player _player;
        private bool _suppressVolumeUpdate;
        [Reactive] public partial ColourService ColourService { get; set; }
        public ICommand? CycleRepeatCommand { get; set; }
        public Player Player { get { return _player; } }
        public ICommand? PlayerNextCommand { get; set; }
        public ICommand? PlayerPlayPauseCommand { get; set; }
        public ICommand? PlayerRepeatOff { get; set; }
        public ICommand? PlayerRepeatQueue { get; set; }
        public ICommand? PlayerRepeatTrack { get; set; }
        public ICommand? PlayingAltMenuCommand { get; set; }
        public ICommand? PlayPreviousCommand { get; set; }

        public QualityTier Quality
        {
            get
            {
                var streamDetails = PlayersService?.SelectedQueue?.CurrentItem?.StreamDetails;
                return PlayerHelpers.DetermineQuality(streamDetails);
            }
        }

        public RelayCommand<double>? SeekCommand { get; }
        public ICommand? ShowTrackInfo { get; set; }
        public DateTime? SleepTimer { get; }
        [Reactive] public partial PlayerQueue? Queue { get; set; }
        [Reactive] public partial PlaybackState State { get; set; }
        public ICommand? ToggleFavoriteCommand { get; set; }
        public ICommand? ToggleShuffleCommand { get; set; }
        [Reactive] public partial double Volume { get; set; }
        [Reactive] public partial double[]? Waveforms { get; set; }

        public PlayerViewModel(Player player, MusicAssistantClient massClient, IScreen screen, PlayersService playersService, ColourService colourService)
            : base(client: massClient,
                  playersService: playersService,
                  screen: screen)
        {
            ColourService = colourService;
            _player = player;

            //TODO: needs to filter to this player only, not the selected player, as this viewmodel is for a specific player.
            Volume = _playersService?.SelectedPlayer?.VolumeLevel ?? 0;

            /* Setup standard actions */
            SetupCommands();

            /* Listen to track change for metadata like waveform and quality. This should be filtered to the current _player*/
            _disposables.Add(
                this.WhenAnyValue(x => x.Queue!.CurrentItem)
                    .Where(currentItem =>
                        !string.IsNullOrEmpty(currentItem?.MediaItem?.ItemId) &&
                        !string.IsNullOrEmpty(currentItem.MediaItem.Provider))
                    .Select(currentItem => Observable.FromAsync(() => FetchWaveformsAsync(currentItem!)))
                    .Switch()
                    .Subscribe(waveforms =>
                    {
                        if (waveforms != null)
                            Waveforms = waveforms!.ToArray();
                        this.RaisePropertyChanged(nameof(Quality));
                    },
                    ex => System.Diagnostics.Debug.WriteLine($"Error loading waveform: {ex.Message}")));

            _disposables.Add(
            this.WhenAnyValue(x => x.PlayersService.SelectedPlayer.VolumeLevel)
            .ObserveOn(AvaloniaScheduler.Instance)
            .Subscribe(
                serverVol =>
                {
                    try
                    {
                        _suppressVolumeUpdate = true;
                        if (serverVol != null)
                            Volume = (int)serverVol;
                    }
                    finally
                    {
                        _suppressVolumeUpdate = false;
                    }
                }));

            /*Lyrics?*/
        }

        public void Dispose()
        {
            _disposables.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SetSleepTimer(Int32 sleepTimer)
        {
            _ = _client.WithWs().PlayersSleepTimerSet(Player.PlayerId!, sleepTimer); ;
        }

        private async Task<List<double>?> FetchWaveformsAsync(QueuedItem currentItem)
        {
            var current = currentItem.MediaItem!;
            var waveforms = await _client.WithWs().GetWaveform(current.ItemId!, current.Provider!);
            return waveforms.Result;
        }

        private void SetupCommands()
        {
            PlayPreviousCommand = new RelayCommand(() => _ = PlayersService.PlayerPrevious(Player));
            PlayerNextCommand = new RelayCommand(() => _ = PlayersService.PlayerNext(Player));
            PlayerPlayPauseCommand = new RelayCommand(() => _ = PlayersService.PlayerPlayPause(Player));
            ToggleShuffleCommand = new RelayCommand(() => _ = PlayersService.PlayerShuffle(Player, !PlayersService.SelectedQueue!.ShuffleEnabled));
            PlayerRepeatQueue = new RelayCommand(() => _ = PlayersService.PlayerSetRepeatMode(RepeatMode.All, Player));
            PlayerRepeatOff = new RelayCommand(() => _ = PlayersService.PlayerSetRepeatMode(RepeatMode.Off, Player));
            PlayerRepeatTrack = new RelayCommand(() => _ = PlayersService.PlayerSetRepeatMode(RepeatMode.One, Player));
            CycleRepeatCommand = new RelayCommand(() =>
            {
                var mode = PlayersService?.SelectedQueue?.RepeatMode;
                switch (mode)
                {
                    case RepeatMode.Off:
                        PlayersService?.PlayerSetRepeatMode(RepeatMode.All);
                        break;

                    case RepeatMode.All:
                        PlayersService?.PlayerSetRepeatMode(RepeatMode.One);
                        break;

                    case RepeatMode.One:
                        PlayersService?.PlayerSetRepeatMode(RepeatMode.Off);
                        break;
                }
            });

            ShowTrackInfo = new RelayCommand(
           () =>
           {
               if (PlayersService != null &&
                   PlayersService.SelectedQueue != null &&
                   PlayersService.SelectedQueue.CurrentItem != null &&
                   PlayersService.SelectedPlayer != null)
                   MessageBus.Current.SendMessage<IPopupViewModel>(new TrackInfoViewModel(PlayersService.SelectedQueue.CurrentItem, PlayersService.SelectedPlayer));
           });

            /*SeekCommand = new RelayCommand<double>(
                (s) =>
                {
                    if (s == 0)
                        return;
                    var duration = _playersService?.SelectedQueue?.CurrentItem?.Duration;
                    var newPosition = duration * (s / 100);
                    if (newPosition != null)
                        _playersService?.PlayerSeek(null, (int)newPosition);
                });*/

            ToggleFavoriteCommand = new RelayCommand(
          () =>
          {
              var item = PlayersService?.SelectedQueue?.CurrentItem?.MediaItem;
              if (item == null)
                  return;

              if (item.Favorite)
                  PlayersService?.PlayerRemoveFromFavorites(item);
              else
                  PlayersService?.PlayerAddToFavorites(item);
          });

            PlayingAltMenuCommand = new RelayCommand(AltMenuAction);
        }

        private void AltMenuAction()
        {
            var item = PlayersService?.SelectedQueue?.CurrentItem?.MediaItem;

            var GoToAlbum = new RelayCommand(() =>
            {
                if (item == null || item.Album == null || item.Album.ItemId == null || item.Provider == null || HostScreen == null)
                    return;

                var albumVm = App.Container.Resolve<AlbumViewModel>();
                albumVm.Album = item.Album;

                string id = string.Empty;
                string provider = string.Empty;
                if (item.Album.ProviderMappings != null)
                {
                    albumVm.Album.ItemId = item.Album.ProviderMappings[0].ItemId!;
                    albumVm.Album.Provider = item.Album.ProviderMappings[0].ProviderDomain!;
                }

                _ = albumVm.LoadAsync();
                HostScreen.Router.Navigate.Execute(albumVm);
            });

            var GoToArtist = new RelayCommand(() =>
            {
                if (item is null || item.Artists is null || item.Provider is null || HostScreen is null)
                    return;
                var artist = item.Artists.FirstOrDefault();
                if (artist is null || artist.ItemId is null)
                    return;

                var artistVm = App.Container.Resolve<ArtistViewModel>();
                _ = artistVm.SetAndLoadModel(artist);
                HostScreen.Router.Navigate.Execute(artistVm);
            });

            var GoToSimilarTracks = new RelayCommand(() =>
            {
                if (item is null || HostScreen is null || item!.ItemId is null)
                    return;

                var SimilarTracksViewModel = App.Container.Resolve<SimilarTracksViewModel>();

                SimilarTracksViewModel.LoadFromId(item.ItemId, item.GetProviderInstance());

                HostScreen.Router.Navigate.Execute(SimilarTracksViewModel);
            });

            if (item != null && item.Album != null && item.Artists != null && PlayersService != null)
            {
                var menu = new MenuViewModel(
                    [
                        new TwoLineMenuItemViewModel("Go to Album", item.Album.Name, PackIconMaterialKind.Album, GoToAlbum),
                            new TwoLineMenuItemViewModel("Go to Artist", item.Artists.FirstOrDefault()!.Name, PackIconMaterialKind.AccountMusic,GoToArtist),
                            new MenuItemViewModel("Similar tracks", PackIconMaterialKind.MusicClefTreble, GoToSimilarTracks),
                            new MenuItemViewModel("Repeat Mode", PackIconMaterialKind.Repeat, null),
                            new MenuItemViewModel("Repeat Off", PackIconMaterialKind.RepeatOff, PlayerRepeatOff, true),
                            new MenuItemViewModel("Repeat Entire Queue",PackIconMaterialKind.RepeatVariant, PlayerRepeatQueue, true),
                            new MenuItemViewModel("Repeat Single Track", PackIconMaterialKind.Repeat, PlayerRepeatTrack, true),
                        ]);

                if (PlayersService?.SelectedQueue?.CurrentItem != null)
                    menu.HeaderItem = PlayersService.SelectedQueue.CurrentItem;

                MessageBus.Current.SendMessage<IPopupViewModel>(menu);
            }
        }
    }
}