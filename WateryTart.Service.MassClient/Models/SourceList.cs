using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class SourceList
{
    [JsonPropertyName("id")]  public string? id { get; set; }
    [JsonPropertyName("name")]  public string? name { get; set; }
    [JsonPropertyName("passive")]  public bool passive { get; set; }
    [JsonPropertyName("can_play_pause")]  public bool can_play_pause { get; set; }
    [JsonPropertyName("can_seek")]  public bool can_seek { get; set; }
    [JsonPropertyName("can_next_previous")]  public bool can_next_previous { get; set; }
}