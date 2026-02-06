using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class MetadataLink
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}