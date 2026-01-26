using System.Collections.ObjectModel;
using WateryTart.MassClient.Models;

namespace WateryTart.Services;

public interface IPlayersService
{
    ObservableCollection<Player> Players { get; }
    void GetPlayers();
    void PlayItem(MediaItemBase t, Player? p = null, PlayerQueue? q = null);
    void PlayerVolumeDown(Player p);
    void PlayerVolumeUp(Player p);
    void PlayerPlayPause(Player p);
}