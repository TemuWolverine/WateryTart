using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Search
{
    [JsonPropertyName("albums")]
    public List<Album>? Albums { get; set; }

    [JsonPropertyName("artists")]
    public List<Artist>? Artists { get; set; }

    [JsonPropertyName("audiobooks")]
    public List<object>? Audiobooks { get; set; }

    [JsonPropertyName("genres")]
    public List<object>? Genres { get; set; }

    [JsonPropertyName("playlists")]
    public List<Playlist>? Playlists { get; set; }

    [JsonPropertyName("podcasts")]
    public List<object>? Podcasts { get; set; }

    [JsonPropertyName("shows")]
    public List<object>? Radio { get; set; }

    [JsonPropertyName("tracks")]
    public List<Item>? Tracks { get; set; }
}