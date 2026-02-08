using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class PlayerQueue : INotifyPropertyChanged
{
    private long? current_index1;
    private string? _state;

    [JsonPropertyName("queue_id")]
    public string? QueueId { get; set; }
    
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }
    
    [JsonPropertyName("available")]
    public bool Available { get; set; }
    
    [JsonPropertyName("items")]
    public Int64 Items { get; set; }
    
    [JsonPropertyName("shuffle_enabled")]
    public bool ShuffleEnabled { get; set; }
    
    [JsonPropertyName("repeat_mode")]
    public string? RepeatMode { get; set; }
    
    [JsonPropertyName("dont_stop_the_music_enabled")]
    public bool DontStopTheMusicEnabled { get; set; }
    
    [JsonPropertyName("current_index")]
    public Int64? CurrentIndex
    {
        get => current_index1;
        set
        {
            current_index1 = value;
            NotifyPropertyChanged();
        }
    }
    
    [JsonPropertyName("index_in_buffer")]
    public Int64? IndexInBuffer { get; set; }
    
    [JsonPropertyName("elapsed_time")]
    public double? ElapsedTime { get; set; }
    
    [JsonPropertyName("elapsed_time_last_updated")]
    public double? ElapsedTimeLastUpdated { get; set; }

    [JsonPropertyName("state")]
    public string? state
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                NotifyPropertyChanged();
            }
        }
    }

    [JsonPropertyName("current_item")]
    public QueuedItem? CurrentItem
    {
        get => field;
        set
        {
            field = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}