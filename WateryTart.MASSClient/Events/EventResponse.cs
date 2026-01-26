using Newtonsoft.Json;
using WateryTart.MassClient.Models;

namespace WateryTart.MassClient.Events;

public class PlayerQueueEventResponse : BaseEventResponse
{
    public PlayerQueue data { get; set; }
}

public class PlayerEventResponse : BaseEventResponse
{
    public Player data { get; set; }
}

public class BaseEventResponse
{
    [JsonProperty("event")]
    public EventType EventName { get; set; }

    public string object_id { get; set; }
}