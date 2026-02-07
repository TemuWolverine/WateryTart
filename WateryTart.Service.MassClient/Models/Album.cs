using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Album : MediaItemBase
{
    [JsonPropertyName("artists")]  public List<Artist>? Artists { get; set; }
    [JsonPropertyName("album_type")] public string? AlbumType { get; set; }
}