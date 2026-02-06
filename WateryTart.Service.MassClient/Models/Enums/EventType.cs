using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Converters;

namespace WateryTart.Service.MassClient.Models.Enums;

[JsonConverter(typeof(FallbackEnumConverter<EventType>))]
public enum EventType
{
    Unknown,
    PlayerAdded,
    PlayerUpdated,
    PlayerRemoved,
    PlayerConfigUpdated,
    QueueAdded,
    QueueUpdated,
    QueueItemsUpdated,
    QueueTimeUpdated,
    MediaItemAdded,
    MediaItemUpdated,
    MediaItemDeleted,
    MediaItemPlayed,
    ProvidersUpdated,
    SyncTasksUpdated,
    ApplicationShutdown
}