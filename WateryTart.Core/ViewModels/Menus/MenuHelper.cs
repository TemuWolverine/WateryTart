using ReactiveUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using WateryTart.Core.Services;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.ViewModels.Menus;

public static class MenuHelper
{
    public static IEnumerable<MenuItemViewModel> AddPlayers(IPlayersService playersService, MediaItemBase item, ReactiveCommand<Unit, Unit> playCommand = null)
    {
        var players = new List<MenuItemViewModel>();

        if (playersService?.Players == null)
            return players;


        var playersList = playersService.Players.ToList();
        if (playersList.Count == 0)
            return players;


        foreach (var p in playersList)
        {
            if (p == null)
            {
                continue;
            }

            var capturedPlayer = p; // Capture for closure
            var capturedItem = item; // Capture item too
            var displayName = capturedPlayer.DisplayName ?? "Unknown Player";

            ICommand playerCommand = new AsyncRelayCommand(async () =>
            {
                Debug.WriteLine($"Playing on {displayName}");
                playersService.SelectedPlayer = capturedPlayer;
                await playersService.PlayItem(capturedItem, capturedPlayer);

            });

            players.Add(new MenuItemViewModel($"\tPlay on {displayName}", string.Empty, playerCommand));
        }

        return players;
    }

    public static MenuViewModel BuildStandardPopup(IPlayersService playersService, MediaItemBase item, bool addPlayers = true)
    {
        if (item == null)
        {
            Debug.WriteLine("BuildStandardPopup: item is null");
            return new MenuViewModel([]);
        }

        if (playersService == null)
        {
            Debug.WriteLine("BuildStandardPopup: playersService is null");
            return new MenuViewModel([]);
        }


        ICommand playCommand = new AsyncRelayCommand(async () =>
        {
            await playersService.PlayItem(item);

        });

        var addToLibraryCommand = new RelayCommand(() => Debug.WriteLine("Add to library clicked"));
        var addToFavouritesCommand = new RelayCommand(() => Debug.WriteLine("Add to favourites clicked"));
        var addToPlaylistCommand = new RelayCommand(() => Debug.WriteLine("Add to playlist clicked"));

        var menu = new MenuViewModel(
        [
            new MenuItemViewModel("Add to library", string.Empty, addToLibraryCommand),
            new MenuItemViewModel("Add to favourites", string.Empty, addToFavouritesCommand),
            new MenuItemViewModel("Add to playlist", string.Empty, addToPlaylistCommand),
            new MenuItemViewModel("Play", string.Empty, playCommand)
        ]);

        if (addPlayers)
        {
            var playerMenuItems = AddPlayers(playersService, item);
            menu.AddMenuItem(playerMenuItems);
        }

        return menu;
    }
}