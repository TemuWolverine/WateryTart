using System.Collections;
using WateryTart.MassClient.Models;

namespace WateryTart.MassClient.Messages;

public class PlayerMessages : MessageFactoryBase
{
    public static MessageBase PlayersAll => JustCommand(Commands.PlayersAll);

    public static MessageBase PlayerQueuesAll => JustCommand(Commands.PlayerQueuesAll);

    public static MessageBase PlayerQueuePlayMedia(string queue_id, MediaItemBase media)
    {
        var m = new Message(Commands.PlayerQueuePlayMedia)
        {
            args = new Hashtable
            {
                { "queue_id", queue_id },
                { "media", media},
                { "option", "play"}
            }
        };

        return m;
    }
}