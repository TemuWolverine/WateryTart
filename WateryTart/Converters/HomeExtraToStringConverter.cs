using System;
using System.Globalization;
using Avalonia.Data.Converters;
using WateryTart.MassClient.Models;

namespace WateryTart.Converters;

public class HomeExtraToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        var item = (Item)value;
        
        switch (item.MediaType)
        {
            case MediaType.Playlist: return item.owner;
            case MediaType.Album: return null;
            case MediaType.Track:
            {
                if (item.artists != null) return item.artists[0].Name ?? string.Empty;
                break;
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}