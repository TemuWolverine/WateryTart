using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using WateryTart.Core.Services;
using WateryTart.Core.ViewModels.Menus;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.ViewModels;

public static class MenuHelper
{
    public static MenuViewModel BuildStandardPopup(IPlayersService _playersService, MediaItemBase item, bool AddPlayers = true)
    {
        if (item == null)
        {
            Debug.WriteLine("BuildStandardPopup: item is null");
            return new MenuViewModel([]);
        }

        if (_playersService == null)
        {
            Debug.WriteLine("BuildStandardPopup: _playersService is null");
            return new MenuViewModel([]);
        }

        var playCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
            Observable.FromAsync(async () => 
            {
                await _playersService.PlayItem(item);
                return Unit.Default;
            })
            .Catch<Unit, Exception>(ex =>
            {
                Debug.WriteLine($"Error playing item: {ex.Message}");
                return Observable.Return(Unit.Default);
            })
        );

        var menu = new MenuViewModel(
        [
            new MenuItemViewModel("Add to library", string.Empty, ReactiveCommand.Create<Unit>(r => {})),
            new MenuItemViewModel("Add to favourites", string.Empty, ReactiveCommand.Create<Unit>(r => { })),
            new MenuItemViewModel("Add to playlist", string.Empty, ReactiveCommand.Create<Unit>(r => { })),
            new MenuItemViewModel("Play", string.Empty, playCommand)
        ]);

        if (AddPlayers)
            menu.AddMenuItem(MenuHelper.AddPlayers(_playersService, item));

        return menu;
    }

    public static IEnumerable<MenuItemViewModel> AddPlayers(IPlayersService _playersService, MediaItemBase item, ReactiveCommand<Unit, Unit> playcommand = null)
    {
        List<MenuItemViewModel> players = [];

        try
        {
            if (_playersService == null)
            {
                Debug.WriteLine("AddPlayers: _playersService is null");
                return players;
            }

            if (_playersService.Players == null)
            {
                Debug.WriteLine("AddPlayers: _playersService.Players is null");
                return players;
            }

            // Ensure we're on the UI thread when accessing the ObservableCollection
            List<Player> playersList = null;
            
            try
            {
                playersList = Dispatcher.UIThread.Invoke(() => 
                {
                    if (_playersService.Players == null)
                    {
                        Debug.WriteLine("AddPlayers: Players collection is null on UI thread");
                        return new List<Player>();
                    }
                    return _playersService.Players.ToList();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddPlayers: Error accessing Players collection: {ex.Message}");
                return players;
            }

            if (playersList == null || playersList.Count == 0)
            {
                Debug.WriteLine($"AddPlayers: No players available (count: {playersList?.Count ?? 0})");
                return players;
            }

            Debug.WriteLine($"AddPlayers: Found {playersList.Count} players");

            foreach (var p in playersList)
            {
                if (p == null)
                {
                    Debug.WriteLine("AddPlayers: Skipping null player");
                    continue;
                }

                var capturedPlayer = p;
                var displayName = capturedPlayer.DisplayName ?? "Unknown Player";
                
                Debug.WriteLine($"AddPlayers: Creating menu item for player: {displayName}");

                ReactiveCommand<Unit, Unit> command = null;
                if (playcommand != null)
                {
                    command = playcommand;
                }
                else
                {
                    command = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
                        Observable.FromAsync(async () =>
                            {
                                _playersService.SelectedPlayer = capturedPlayer;
                                await _playersService.PlayItem(item, capturedPlayer);
                                return Unit.Default;
                            })
                            .Catch<Unit, Exception>(ex =>
                            {
                                Debug.WriteLine($"Error playing item on {displayName}: {ex.Message}");
                                return Observable.Return(Unit.Default);
                            })
                    );
                }

                players.Add(new MenuItemViewModel($"\tPlay on {displayName}", string.Empty, command));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in AddPlayers: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        return players;
    }
}