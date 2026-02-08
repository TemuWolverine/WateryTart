using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class MediaItem : Item, INotifyPropertyChanged
{
    [JsonPropertyName("elapsed_time")]
    public double ElapsedTime
    {
        get => field;
        set
        {
            field = value;
            NotifyPropertyChanged();
        }
    } = 0;
    public double Progress
    {
        get
        {
            if (Duration == null || Duration == 0)
                    return 0;
            return (ElapsedTime / Duration.Value) * 100;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}