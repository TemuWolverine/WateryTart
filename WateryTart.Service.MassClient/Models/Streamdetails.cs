using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Streamdetails
{
    [JsonPropertyName("provider")] public string? Provider { get; set; }
    [JsonPropertyName("item_id")] public string? ItemId { get; set; }
    [JsonPropertyName("audio_format")] public AudioFormat? AudioFormat { get; set; }
    [JsonPropertyName("media_type")] public string? MediaType { get; set; }
    [JsonPropertyName("stream_type")] public string? StreamType { get; set; }
    [JsonPropertyName("duration")] public int Duration { get; set; }
    [JsonPropertyName("size")] public object? Size { get; set; }
    [JsonPropertyName("stream_metadata")] public object? StreamMetadata { get; set; }
    [JsonPropertyName("loudness")] public object? Loudness { get; set; }
    [JsonPropertyName("loudness_album")] public object? LoudnessAlbum { get; set; }
    [JsonPropertyName("prefer_album_loudness")] public bool PreferAlbumLoudness { get; set; }
    [JsonPropertyName("volume_normalization_mode")] public string? VolumeNormalizationMode { get; set; }
    [JsonPropertyName("volume_normalization_gain_correct")] public object? VolumeNormalizationGainCorrect { get; set; }
    [JsonPropertyName("target_loudness")] public double TargetLoudness { get; set; }

    // public Dsp dsp { get; set; }
    [JsonPropertyName("stream_title")] public object? StreamTitle { get; set; }
}