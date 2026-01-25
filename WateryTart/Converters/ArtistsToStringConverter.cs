using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;
using WateryTart.MassClient.Models;

namespace WateryTart.Converters;

public class ArtistsToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        IList<Artist> artists = (IList<Artist>)value;

        StringBuilder sb = new StringBuilder();
        foreach (var a in artists)
            sb.Append(a.Name);

        return sb.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}