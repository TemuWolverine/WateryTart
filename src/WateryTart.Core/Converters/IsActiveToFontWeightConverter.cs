using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace WateryTart.Core.Converters
{
    public class IsActiveToFontWeightConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isActive && isActive)
                return FontWeight.Bold;

            return FontWeight.Normal;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

