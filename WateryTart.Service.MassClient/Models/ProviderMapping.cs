using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;
public class ProviderMapping
{
    [JsonPropertyName("item_id")]
    public string? ItemId { get; set; }
    
    [JsonPropertyName("provider_domain")]
    public string? ProviderDomain { get; set; }
    
    [JsonPropertyName("provider_instance")]
    public string? ProviderInstance { get; set; }
    
    [JsonPropertyName("available")]
    public bool Available { get; set; }
    
    [JsonPropertyName("in_library")]
    public bool? InLibrary { get; set; }
    
    [JsonPropertyName("is_unique")]
    public bool? IsUnique { get; set; }
    
    [JsonPropertyName("audio_format")]
    public AudioFormat? AudioFormat { get; set; }
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("details")]
    public object? Details { get; set; }
}