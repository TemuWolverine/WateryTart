using WateryTart.Service.MassClient.Messages;
using WateryTart.Service.MassClient.Models;
using WateryTart.Service.MassClient.Models.Enums;
using WateryTart.Service.MassClient.Responses;

namespace WateryTart.Service.MassClient;

public static partial class MassClientExtensions
{
    extension(MassRpcClient c)
    {

        /* Player related commands */
        public async Task<List<Player>?> PlayersAllAsync()
        {
            return await c.Send<List<Player>>(JustCommand(Commands.PlayersAll));
        }

        public async Task<List<PlayerQueue>?> PlayerNextAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerNext, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerPlayAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerPlay, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerPlayPauseAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerPlayPause, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerPreviousAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerPrevious, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerQueuesAllAsync()
        {
            return await c.Send<List<PlayerQueue>>(JustCommand(Commands.PlayerQueuesAll));
        }

        public async Task<PlayerQueue?> PlayerActiveQueueAsync(string id)
        {
            return await c.Send<PlayerQueue>(JustId(Commands.PlayerActiveQueue, id, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerQueueItemsAsync(string id)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerQueueItems, id, "queue_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerGroupVolumeUpAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerGroupVolumeUp, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayerGroupVolumeDownAsync(string playerId)
        {
            return await c.Send<List<PlayerQueue>>(JustId(Commands.PlayerGroupVolumeDown, playerId, "player_id"));
        }

        public async Task<List<PlayerQueue>?> PlayAsync(string queueId, MediaItemBase t, PlayMode mode, bool radiomode)
        {
            var modestr = mode switch
            {
                PlayMode.Play => "play",
                PlayMode.Replace => "replace",
                PlayMode.Next => "next",
                PlayMode.ReplaceNext => "replace_next",
                PlayMode.Add => "add",
                _ => "unknown"
            };

            var mediaArray = new string?[] { t.Uri };

            var m = new Message(Commands.PlayerQueuePlayMedia)
            {
                args = new Dictionary<string, object>()
                {
                    { "queue_id", queueId },
                    { "media", mediaArray },
                    { "option", modestr }
                }
            };

            if (radiomode)
                m.args.Add("radio_mode", true);

            return await c.Send<List<PlayerQueue>>(m);
        }



        /*Search*/
        public async Task<Search?> SearchAsync(string query, int? limit = null, bool library_only = true)
        {
            var args = new Dictionary<string, object>()
            {
                { "search_query", query },
                { "library_only", library_only }
            };

            if (limit != null)
                args.Add("limit", limit);

            var m = new Message(Commands.Search)
            {
                args = args
            };

            var y = await c.Send<Search>(m);
            return y;
        }
    }
}
