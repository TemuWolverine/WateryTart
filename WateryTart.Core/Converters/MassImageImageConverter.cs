using System;
using System.Globalization;
using Avalonia.Data.Converters;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.Converters;

public class MassImageImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Image image = (Image)value;
        if (image.remotely_accessible)
            return image.path;
        else
            return ProxyString(image.path, image.provider); 
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string ProxyString(string path, string provider)
    {
        return string.Format("http://{0}/imageproxy?path={1}&provider={2}&checksum=&size=256", App.BaseUrl, Uri.EscapeDataString(path), provider);
    }
}