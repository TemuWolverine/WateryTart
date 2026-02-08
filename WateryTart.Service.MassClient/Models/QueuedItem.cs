using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class QueuedItem
{
    [JsonPropertyName("queue_id")]
    public string? QueueId { get; set; }
    
    [JsonPropertyName("queue_item_id")]
    public string? QueueItemId { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
    
    [JsonPropertyName("sort_index")]
    public int SortIndex { get; set; }
    
    [JsonPropertyName("media_item")]
    public MediaItem? MediaItem { get; set; }
    
    [JsonPropertyName("image")]
    public Image? Image { get; set; }

    [JsonPropertyName("StreamDetails")]
    public Streamdetails? StreamDetails { get; set; }
}



