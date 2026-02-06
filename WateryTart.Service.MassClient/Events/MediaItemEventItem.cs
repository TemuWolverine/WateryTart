using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Events;

public class MediaItemEventItem 
{
    [JsonPropertyName("uri")]
    public string uri { get; set; }
    
    [JsonPropertyName("media_type")]
    public string media_type { get; set; }
    
    [JsonPropertyName("name")]
    public string name { get; set; }
    
    [JsonPropertyName("duration")]
    public int duration { get; set; }
    
    [JsonPropertyName("seconds_played")]
    public int seconds_played { get; set; }
    
    [JsonPropertyName("fully_played")]
    public bool fully_played { get; set; }
    
    [JsonPropertyName("is_playing")]
    public bool is_playing { get; set; }
    
    [JsonPropertyName("mbid")]
    public object mbid { get; set; }
    
    [JsonPropertyName("artist")]
    public string artist { get; set; }
    
    [JsonPropertyName("artists")]
    public List<string> artists { get; set; }
    
    [JsonPropertyName("artist_mbids")]
    public List<object> artist_mbids { get; set; }
    
    [JsonPropertyName("album")]
    public string album { get; set; }
    
    [JsonPropertyName("album_mbid")]
    public object album_mbid { get; set; }
    
    [JsonPropertyName("album_artist")]
    public string album_artist { get; set; }
    
    [JsonPropertyName("album_artist_mbids")]
    public List<object> album_artist_mbids { get; set; }
    
    [JsonPropertyName("image_url")]
    public string image_url { get; set; }
    
    [JsonPropertyName("version")]
    public string version { get; set; }
    
    [JsonPropertyName("userid")]
    public string userid { get; set; }
}