using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Models.Enums;

namespace WateryTart.Service.MassClient.Models;

public class Image
{
    [JsonPropertyName("type")]
    public ImageType type { get; set; }
    
    [JsonPropertyName("path")]
    public string? path { get; set; }
    
    [JsonPropertyName("provider")]
    public string? provider { get; set; }
    
    [JsonPropertyName("remotely_accessible")]
    public bool remotely_accessible { get; set; }
}