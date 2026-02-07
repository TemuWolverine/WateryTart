using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using WateryTart.Service.MassClient.Models;

namespace WateryTart.Core.Converters;

public class MetadataImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            var item = value as MediaItemBase;
            if (item == null)
                return null;

            //If it is an item, but has a "image" field set, use that
            if (item.image != null && !string.IsNullOrEmpty(item.image.path))
                //If the image field starts with http, use that
                if (item.image.provider != null)
                    return item.image.path.StartsWith(("http"))
                        ? item.image.path
                        : ProxyString(item.image.path, item.image.provider);

            //If there is no image field set, use metadata, make sure its not null
            if (item.Metadata?.Images == null)
                return null;

            //Try a locally accessible source first
            var result = item.Metadata.Images.FirstOrDefault(i => !i.remotely_accessible);
            if (result == null)
                result = item.Metadata.Images.FirstOrDefault(i => i.remotely_accessible);

            if (result?.provider != null)
                if (result.path != null)
                    return result.path != null && result.path.StartsWith("http")
                        ? result.path
                        : ProxyString(result.path, result.provider);
        }
        catch (Exception ex)
        {
            App.Logger?.LogError(ex, "image metadata failure");
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }

    private static string ProxyString(string path, string provider)
    {
        return $"http://{App.BaseUrl}/imageproxy?path={Uri.EscapeDataString(path)}&provider={provider}&checksum=&size=256";
    }
}