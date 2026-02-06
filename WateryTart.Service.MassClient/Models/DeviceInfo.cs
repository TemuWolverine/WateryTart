using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class DeviceInfo
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    
    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }
}