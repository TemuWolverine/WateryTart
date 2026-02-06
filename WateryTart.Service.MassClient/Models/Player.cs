using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using WateryTart.Service.MassClient.Models.Enums;

namespace WateryTart.Service.MassClient.Models;

public partial class Player : INotifyPropertyChanged, IResult
{
    private CurrentMedia? currentMedia;
    private int? volumeLevel;

    [JsonPropertyName("player_id")] public string? PlayerId { get; set; }
    public string? Provider { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
    public bool Available { get; set; }
    [JsonPropertyName("device_info")] public DeviceInfo? DeviceInfo { get; set; }
    [JsonPropertyName("supported_features")] public List<string>? SupportedFeatures { get; set; }

    [JsonPropertyName("playback_state")]
    public PlaybackState PlaybackState
    {
        get => field;
        set
        {
            field = value;
            NotifyPropertyChanged();
        }
    }

    [JsonPropertyName("elapsed_time")] public double? ElapsedTime { get; set; }
    [JsonPropertyName("elapsed_time_last_updated")] public double? ElapsedTimeLastUpdated { get; set; }
    public bool Powered { get; set; }
    [JsonPropertyName("volume_level")]
    public int? VolumeLevel
    {
        get => volumeLevel; set
        {
            volumeLevel = value;
            NotifyPropertyChanged();
        }
    }
    [JsonPropertyName("volume_muted")] public bool? VolumeMuted { get; set; }
    [JsonPropertyName("can_group_with")] public List<string>? CanGroupWith { get; set; }
    [JsonPropertyName("synced_to")] public object? SyncedTo { get; set; }
    [JsonPropertyName("active_source")] public string? ActiveSource { get; set; }
    [JsonPropertyName("source_list")] public List<SourceList>? SourceList { get; set; }
    [JsonPropertyName("active_group")] public object? ActiveGroup { get; set; }
    [JsonPropertyName("current_media")]
    public CurrentMedia? CurrentMedia
    {
        get => currentMedia;
        set
        {
            currentMedia = value;
            NotifyPropertyChanged();
        }
    }
    public bool Enabled { get; set; }
    [JsonPropertyName("hide_player_in_ui")] public List<string>? HidePlayerInUI { get; set; }
    [JsonPropertyName("expose_to_ha")] public bool ExposedToHA { get; set; }
    public string? Icon { get; set; }
    [JsonPropertyName("group_volume")] public int? GroupVolume { get; set; }
    [JsonPropertyName("extra_attributes")] public ExtraAttributes? ExtraAttributes { get; set; }
    [JsonPropertyName("power_control")] public string? PowerControl { get; set; }
    [JsonPropertyName("volume_control")] public string? VolumeControl { get; set; }
    [JsonPropertyName("mute_control")] public string? MuteControl { get; set; }

    [JsonPropertyName("display_name")] public string? DisplayName { get; set; }
    public string? state { get; set; }
    [JsonPropertyName("group_childs")] public List<object>? GroupChilds { get; set; }
    [JsonPropertyName("extra_data")] public ExtraData? ExtraData { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}