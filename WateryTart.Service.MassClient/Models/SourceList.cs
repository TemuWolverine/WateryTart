using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class SourceList
{
    [JsonPropertyName("id")]  public string? Id { get; set; }
    [JsonPropertyName("name")]  public string? Name { get; set; }
    [JsonPropertyName("passive")]  public bool Passive { get; set; }
    [JsonPropertyName("can_play_pause")]  public bool CanPlayPause { get; set; }
    [JsonPropertyName("can_seek")]  public bool CanSeek { get; set; }
    [JsonPropertyName("can_next_previous")]  public bool CanNextPrevious { get; set; }
}