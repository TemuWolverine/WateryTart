using Newtonsoft.Json;
using System.Collections;
using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;

namespace WateryTart.MassClient;


public static partial class MassClientExtensions
{
    extension(IMassWsClient c)
    {
        public void Play(string queueID, MediaItemBase t, PlayMode mode, Action<PlayersQueuesResponse> responseHandler)
        {
            /* Unsure why this didn't serialise the enum, perhaps because it was in a hashtable */
            var modestr = "";
            switch (mode)
            {
                case PlayMode.Play:
                    modestr = "play";
                    break;
                case PlayMode.Replace:
                    modestr = "replace";
                    break;
                case PlayMode.Next:
                    modestr = "next";
                    break;
                case PlayMode.ReplaceNext:
                    modestr = "replace_next";
                    break;
                case PlayMode.Add:
                    modestr = "add";
                    break;
                case PlayMode.Unknown:
                    modestr = "unknown";
                    break;
            }

            var m = new Message(Commands.PlayerQueuePlayMedia)
            {
                args = new Hashtable
                {
                    { "queue_id", queueID },
                    { "media", t},
                    { "option", mode}
                }
            };


            c.Send<PlayerResponse>(m, Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayAsync(string queueID, MediaItemBase t, PlayMode mode)
        {
            var modestr = "";
            switch (mode)
            {
                case PlayMode.Play:
                    modestr = "play";
                    break;
                case PlayMode.Replace:
                    modestr = "replace";
                    break;
                case PlayMode.Next:
                    modestr = "next";
                    break;
                case PlayMode.ReplaceNext:
                    modestr = "replace_next";
                    break;
                case PlayMode.Add:
                    modestr = "add";
                    break;
                case PlayMode.Unknown:
                    modestr = "unknown";
                    break;
            }

            var m = new Message(Commands.PlayerQueuePlayMedia)
            {
                args = new Hashtable
                {
                    { "queue_id", queueID },
                    { "media", t},
                    { "option", mode}
                }
            };

            return await SendAsync<PlayersQueuesResponse>(c, m);
        }

        public void PlayerNext(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerNext, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerNextAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerNext, playerId, "player_id"));
        }

        public void PlayerPlay(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerPlay, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerPlayAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerPlay, playerId, "player_id"));
        }

        public void PlayerPlayPause(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerPlayPause, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerPlayPauseAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerPlayPause, playerId, "player_id"));
        }

        public void PlayerPrevious(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerPrevious, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerPreviousAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerPrevious, playerId, "player_id"));
        }

        public void PlayersAll(Action<PlayerResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustCommand(Commands.PlayersAll), Deserialise<PlayerResponse>(responseHandler));
        }

        public async Task<PlayerResponse> PlayersAllAsync()
        {
            return await SendAsync<PlayerResponse>(c, JustCommand(Commands.PlayersAll));
        }

        public void PlayerQueuesAll(Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustCommand(Commands.PlayerQueuesAll), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerQueuesAllAsync()
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustCommand(Commands.PlayerQueuesAll));
        }

        public void PlayerActiveQueue(string id, Action<PlayerQueueResponse> responseHandler)
        {
            c.Send<PlayerQueueResponse>(JustId(Commands.PlayerActiveQueue, id, "player_id"), Deserialise<PlayerQueueResponse>(responseHandler));
        }

        public async Task<PlayerQueueResponse> PlayerActiveQueueAsync(string id)
        {
            return await SendAsync<PlayerQueueResponse>(c, JustId(Commands.PlayerActiveQueue, id, "player_id"));
        }

        public void PlayerQueueItems(string id, Action<PlayerQueueItemsResponse> responseHandler)
        {
            c.Send<PlayerQueueItemsResponse>(JustId(Commands.PlayerQueueItems, id, "queue_id"), Deserialise<PlayerQueueItemsResponse>(responseHandler));
        }

        public async Task<PlayerQueueItemsResponse> PlayerQueueItemsAsync(string id)
        {
            return await SendAsync<PlayerQueueItemsResponse>(c, JustId(Commands.PlayerQueueItems, id, "queue_id"));
        }

        public void PlayerGroupVolumeUp(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerGroupVolumeUp, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerGroupVolumeUpAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerGroupVolumeUp, playerId, "player_id"));
        }

        public void PlayerGroupVolumeDown(string playerId, Action<PlayersQueuesResponse> responseHandler)
        {
            c.Send<PlayerResponse>(JustId(Commands.PlayerGroupVolumeDown, playerId, "player_id"), Deserialise<PlayersQueuesResponse>(responseHandler));
        }

        public async Task<PlayersQueuesResponse> PlayerGroupVolumeDownAsync(string playerId)
        {
            return await SendAsync<PlayersQueuesResponse>(c, JustId(Commands.PlayerGroupVolumeDown, playerId, "player_id"));
        }
    }
}
