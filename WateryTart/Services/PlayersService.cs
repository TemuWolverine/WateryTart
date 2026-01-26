using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using WateryTart.Extensions;
using WateryTart.MassClient;
using WateryTart.MassClient.Events;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;

namespace WateryTart.Services;

public partial class PlayersService : ReactiveObject, IPlayersService
{
    private readonly IMassWsClient _massClient;

    [Reactive] public partial ObservableCollection<Player> Players { get; set; }
    [Reactive] public partial ObservableCollection<PlayerQueue> Queues { get; set; }
    [Reactive] public partial Player SelectedPlayer { get; set; }
    
    public PlayersService(IMassWsClient massClient)
    {
        _massClient = massClient;

        Players = new ObservableCollection<Player>();
        Queues = new ObservableCollection<PlayerQueue>();

        _massClient.Events
            .Where(e => e is PlayerEventResponse)
            .Subscribe((e) => OnPlayerEvents((PlayerEventResponse)e));

        _massClient.Events
            .Where(e => e is PlayerQueueEventResponse)
            .Subscribe((e) => OnPlayerQueueEvents((PlayerQueueEventResponse)e));
    }

    public void OnPlayerQueueEvents(PlayerQueueEventResponse e)
    {

    }

    public void OnPlayerEvents(PlayerEventResponse e)
    {
        Debug.WriteLine(e.EventName);

        switch (e.EventName)
        {
            case EventType.PlayerAdded:
                if (!Players.Contains((e.data))) //Any?
                    Players.Add(e.data);

                break;
            case EventType.PlayerUpdated:
                if (e.data.Available == false)
                    Players.RemoveAll(p=> p.PlayerId == e.data.PlayerId);

                break;
            case EventType.PlayerRemoved:
                Players.RemoveAll(p => p.PlayerId == e.data.PlayerId);
                break;
            default:

                break;
        }
    }

    public void GetPlayers()
    {
        _massClient
            .PlayersAll((a) =>
            {
                foreach (var y in a.Result)
                {
                    Players.Add(y);
                }
            });

        _massClient.PlayerQueuesAll(a =>
        {
            foreach (var y in a.Result)
            {
                Queues.Add(y);
                Debug.WriteLine(y.display_name);
            }
        });
    }

    public void PlayerVolumeDown(Player p)
    {
    }

    public void PlayerVolumeUp(Player p)
    {
    }

    public void PlayerPlayPause(Player p)
    {

    }
    public void PlayItem(MediaItemBase t, Player? p = null, PlayerQueue? q = null)
    {
        /*{
             "command": "player_queues/get_active_queue",
             "args": {
               "player_id": "example_value"
             }
           }*/

        /*
        if (p == null)
            p = Players.FirstOrDefault(x => x.DisplayName == "Web (Firefox on Windows)");

        if (q == null)
        {
            q = Queues.FirstOrDefault(pq => pq.display_name == p.DisplayName);
            _massClient.Play(q.queue_id, t, (a) =>
            {
                Debug.WriteLine(a.Result);
            });

        }
        */
        var w = Queues.FirstOrDefault(pq => pq.display_name == "Web (Firefox on Windows)");
        
        _massClient.Play(w.queue_id, t, (a) =>
        {
            Debug.WriteLine(a);
        });
    }
}