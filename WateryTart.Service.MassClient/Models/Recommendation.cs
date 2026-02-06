using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Recommendation : MediaItemBase
{
    [JsonPropertyName("path")] public string? path { get; set; }
    [JsonPropertyName("image")] public new object? image { get; set; }
    [JsonPropertyName("icon")] public string? icon { get; set; }
    [JsonPropertyName("items")] public List<Item>? items { get; set; }
    [JsonPropertyName("subtitle")] public string? subtitle { get; set; }
}