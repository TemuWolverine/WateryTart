using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;
public class AudioFormat
{
    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }
    
    [JsonPropertyName("codec_type")]
    public string? CodecType { get; set; }
    
    [JsonPropertyName("sample_rate")]
    public int SampleRate { get; set; }
    
    [JsonPropertyName("bit_depth")]
    public int? BitDepth { get; set; }
    
    [JsonPropertyName("channels")]
    public int? Channels { get; set; }
    
    [JsonPropertyName("output_format_str")]
    public string? OutputFormatStr { get; set; }
    
    [JsonPropertyName("bit_rate")]
    public int? BitRate { get; set; }
}