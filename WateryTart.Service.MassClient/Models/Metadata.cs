using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Metadata
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("review")]
    public string? Review { get; set; }
    
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; set; }
    
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }
    
    [JsonPropertyName("grouping")]
    public string? Grouping { get; set; }
    
    [JsonPropertyName("genres")]
    public List<string>? Genres { get; set; }
    
    [JsonPropertyName("mood")]
    public string? Mood { get; set; }
    
    [JsonPropertyName("style")]
    public string? Style { get; set; }
    
    [JsonPropertyName("copyright")]
    public string? Copyright { get; set; }
    
    [JsonPropertyName("lyrics")]
    public string? Lyrics { get; set; }
    
    [JsonPropertyName("lrc_lyrics")]
    public string? LrcLyrics { get; set; }
    
    [JsonPropertyName("label")]
    public string? Label { get; set; }
    
    [JsonPropertyName("links")]
    public List<MetadataLink>? Links { get; set; }
    
    [JsonPropertyName("performers")]
    public object? Performers { get; set; }
    
    [JsonPropertyName("preview")]
    public object? Preview { get; set; }
    
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }
    
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
    
    [JsonPropertyName("languages")]
    public object? Languages { get; set; }
    
    [JsonPropertyName("chapters")]
    public object? Chapters { get; set; }
    
    [JsonPropertyName("last_refresh")]
    public long? LastRefresh { get; set; }
}