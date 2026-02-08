using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Models.Enums;

namespace WateryTart.Service.MassClient.Models;

public class Image
{
    [JsonPropertyName("type")]
    public ImageType Type { get; set; }
    
    [JsonPropertyName("path")]
    public string? Path { get; set; }
    
    [JsonPropertyName("provider")]
    public string? Provider { get; set; }
    
    [JsonPropertyName("remotely_accessible")]
    public bool RemotelyAccessible { get; set; }
}