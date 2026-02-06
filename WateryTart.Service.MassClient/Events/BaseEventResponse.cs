using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Models.Enums;

namespace WateryTart.Service.MassClient.Events;

public class BaseEventResponse
{
    [JsonPropertyName("event")] public EventType EventName { get; set; }
    
    [JsonPropertyName("object_id")] public string? object_id { get; set; }
    
    [JsonPropertyName("data")] public object? data { get; set; }
}