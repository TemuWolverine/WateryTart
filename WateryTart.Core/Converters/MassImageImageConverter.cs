using System;
using System.Globalization;
using Avalonia.Data.Converters;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.Converters;

public class MassImageImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Image? image = (Image)value;

        if (image == null)
            return string.Empty;

        if (image.remotely_accessible)
            return image.path;
        else if (image.path != null)
            if (image.provider != null)
                return ProxyString(image.path, image.provider);

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }

    private static string ProxyString(string path, string provider)
    {
        return string.Format("http://{0}/imageproxy?path={1}&provider={2}&checksum=&size=256", App.BaseUrl, Uri.EscapeDataString(path), provider);
    }
}