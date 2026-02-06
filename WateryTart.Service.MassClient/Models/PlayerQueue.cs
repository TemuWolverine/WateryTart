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
    public string? queue_id { get; set; }
    
    [JsonPropertyName("active")]
    public bool active { get; set; }
    
    [JsonPropertyName("display_name")]
    public string? display_name { get; set; }
    
    [JsonPropertyName("available")]
    public bool available { get; set; }
    
    [JsonPropertyName("items")]
    public Int64 items { get; set; }
    
    [JsonPropertyName("shuffle_enabled")]
    public bool shuffle_enabled { get; set; }
    
    [JsonPropertyName("repeat_mode")]
    public string? repeat_mode { get; set; }
    
    [JsonPropertyName("dont_stop_the_music_enabled")]
    public bool dont_stop_the_music_enabled { get; set; }
    
    [JsonPropertyName("current_index")]
    public Int64? current_index
    {
        get => current_index1;
        set
        {
            current_index1 = value;
            NotifyPropertyChanged();
        }
    }
    
    [JsonPropertyName("index_in_buffer")]
    public Int64? index_in_buffer { get; set; }
    
    [JsonPropertyName("elapsed_time")]
    public double? elapsed_time { get; set; }
    
    [JsonPropertyName("elapsed_time_last_updated")]
    public double? elapsed_time_last_updated { get; set; }

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
    public QueuedItem? current_item
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