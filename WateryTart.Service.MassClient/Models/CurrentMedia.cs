using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Models.Enums;

namespace WateryTart.Service.MassClient.Models;

public partial class CurrentMedia : INotifyPropertyChanged
{
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
    
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("artist")]
    public string? Artist { get; set; }
    
    [JsonPropertyName("album")]
    public string? Album { get; set; }
    
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }
    
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
    
    [JsonPropertyName("queue_id")]
    public string? QueueId { get; set; }
    
    [JsonPropertyName("queue_item_id")]
    public string? QueueItemId { get; set; }

    [JsonPropertyName("elapsed_time")]
    public double? ElapsedTime
    {
        get => field;
        set
        {
            if (value.HasValue)
            {
                field = value.Value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Progress");
            }
        }
    }
    [JsonPropertyName("progress")]
    public double Progress
    {
        get
        {
            if (Duration == null || Duration == 0 || ElapsedTime == null)
                return 0;
            
            return ((double)ElapsedTime / (double)Duration) * 100;
        }
    }

    [JsonPropertyName("elapsed_time_last_updated")]
    public double? elapsed_time_last_updated { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}