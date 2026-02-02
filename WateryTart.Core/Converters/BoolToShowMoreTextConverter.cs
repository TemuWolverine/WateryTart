using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace WateryTart.Core.Converters;

public class BoolToShowMoreTextConverter : IValueConverter
{
    public static readonly BoolToShowMoreTextConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isExpanded)
        {
            return isExpanded ? "Show less ▲" : "Show more ▼";
        }
        return "Show more ▼";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}