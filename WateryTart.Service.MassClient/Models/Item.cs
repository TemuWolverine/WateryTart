using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Item : MediaItemBase
{
    [JsonPropertyName("position")]  public object? Position { get; set; }
    [JsonPropertyName("owner")] public string? Owner { get; set; }
    [JsonPropertyName("is_editable")] public bool? IsEditable { get; set; }
    [JsonPropertyName("available")] public bool? Available { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
    [JsonPropertyName("artists")] public List<Artist>? Artists { get; set; }
    [JsonPropertyName("last_played")] public int? LastPlayed { get; set; }
    [JsonPropertyName("album")] public Album? Album { get; set; }
    [JsonPropertyName("disc_number")] public int? DiscNumber { get; set; }
    [JsonPropertyName("track_number")] public int? TrackNumber { get; set; }
    [JsonPropertyName("album_type")] public string? AlbumType { get; set; }
}
