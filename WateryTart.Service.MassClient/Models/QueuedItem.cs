using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class QueuedItem
{
    [JsonPropertyName("queue_id")]
    public string? queue_id { get; set; }
    
    [JsonPropertyName("queue_item_id")]
    public string? queue_item_id { get; set; }
    
    [JsonPropertyName("name")]
    public string? name { get; set; }
    
    [JsonPropertyName("duration")]
    public int? duration { get; set; }
    
    [JsonPropertyName("sort_index")]
    public int sort_index { get; set; }
    
    [JsonPropertyName("media_item")]
    public MediaItem? media_item { get; set; }
    
    [JsonPropertyName("image")]
    public Image? image { get; set; }
    public Streamdetails? streamdetails { get; set; }
}



