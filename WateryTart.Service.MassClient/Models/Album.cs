using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Album : MediaItemBase
{
    [JsonPropertyName("version")] public string? Version { get; set; }
    [JsonPropertyName("artists")]  public List<Artist>? Artists { get; set; }
    [JsonPropertyName("year")] public int? Year { get; set; }
    [JsonPropertyName("album_type")] public string? AlbumType { get; set; }
}