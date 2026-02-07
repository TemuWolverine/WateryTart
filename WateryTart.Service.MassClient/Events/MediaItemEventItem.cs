using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Events;

public class MediaItemEventItem 
{
    [JsonPropertyName("uri")]
    public string? uri { get; set; }
    
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
    
    [JsonPropertyName("seconds_played")]
    public int? SecondsPlayed { get; set; }
    
    [JsonPropertyName("fully_played")]
    public bool? FullyPlayed { get; set; }
    
    [JsonPropertyName("is_playing")]
    public bool? IsPlaying { get; set; }
    
    [JsonPropertyName("mbid")]
    public object? Mbid { get; set; }
    
    [JsonPropertyName("artist")]
    public string? Artist { get; set; }
    
    [JsonPropertyName("artists")]
    public List<string>? Artists { get; set; }
    
    [JsonPropertyName("artist_mbids")]
    public List<object>? ArtistMbids { get; set; }
    
    [JsonPropertyName("album")]
    public string? Album { get; set; }
    
    [JsonPropertyName("album_mbid")]
    public object? AlbumMbid { get; set; }
    
    [JsonPropertyName("album_artist")]
    public string? AlbumArtist { get; set; }
    
    [JsonPropertyName("album_artist_mbids")]
    public List<object>? AlbumArtistMbids { get; set; }
    
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }
    
    [JsonPropertyName("version")]
    public string? Version { get; set; }
    
    [JsonPropertyName("userid")]
    public string? Userid { get; set; }
}