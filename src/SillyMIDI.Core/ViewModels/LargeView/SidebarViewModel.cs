using Autofac;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.Input;
using IconPacks.Avalonia.Material;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using SillyMIDI.Core.Services;
using SillyMIDI.Core.Views;
using SillyMIDI.MusicAssistant;
using SillyMIDI.MusicAssistant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace SillyMIDI.Core.ViewModels.LargeView
{
    public class SidebarHeaderModel : SidebarModel
    {

    }
    public class SidebarModel
    {
        public bool IsHeading { get; set; } = false;
        public required string Name { get; set; }
        public PackIconMaterialKind Icon { get; set; } = PackIconMaterialKind.Abacus;
        public ICommand? Action { get; set; } = default;
    }
    public class SidebarViewModel : ViewModelBase<SidebarViewModel>
    {
        public ObservableCollection<SidebarModel> Items { get; set; }
        public SidebarViewModel(MusicAssistantClient massClient, IScreen screen, PlayersService? playersService)
        : base(null, massClient, playersService, screen)
        {
            Items =
            [
                new SidebarHeaderModel { Name = "Explore", IsHeading = true},
                new SidebarModel { Name = "Discover", Icon = PackIconMaterialKind.Compass },
                new SidebarModel { Name = "Search", Icon = PackIconMaterialKind.Magnify },
                new SidebarModel { Name = "Browse", Icon = PackIconMaterialKind.Folder },

                
                new SidebarHeaderModel { Name = "Library", IsHeading = true},
                new SidebarModel { Name = "Artists", Icon = PackIconMaterialKind.AccountMusic, Action = new RelayCommand(NavigateToArtists) },
                new SidebarModel { Name = "Albums", Icon = PackIconMaterialKind.Album, Action = new RelayCommand(NavigateToAlbums) },
                new SidebarModel { Name = "Tracks", Icon = PackIconMaterialKind.MusicNote },
                new SidebarModel { Name = "Playlists", Icon = PackIconMaterialKind.PlaylistMusic },
                new SidebarModel { Name = "Radio", Icon = PackIconMaterialKind.Radio },
                new SidebarModel { Name = "Audioboos", Icon = PackIconMaterialKind.BookMusic },
                new SidebarModel { Name = "Podcasts", Icon = PackIconMaterialKind.Podcast },
                new SidebarModel { Name = "Genres", Icon = PackIconMaterialKind.MusicBoxMultiple },


                new SidebarHeaderModel { Name = "All Playlists", IsHeading = true},
            ];
        }

        private void NavigateToArtists()
        {
            var vm = new LoadMoreListViewModel<ArtistViewModel>(_client, HostScreen, _playersService!, App.Container.Resolve<ILoggerFactory>(), "Artists", true);
            HostScreen.Router.Navigate.Execute(vm);

        }
        private void NavigateToAlbums()
        {
            var vm = new LoadMoreListViewModel<AlbumViewModel>(_client, HostScreen, _playersService!, App.Container.Resolve<ILoggerFactory>(), "Albums", true);
            HostScreen.Router.Navigate.Execute(vm);
        }
    }
}
