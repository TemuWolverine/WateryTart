using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WateryTart.Utilities;

namespace WateryTart.Services;

public partial class ColourService : ReactiveObject, IColourService
{
    [Reactive] public partial Color ColourA { get; set; }
    [Reactive] public partial Color ColourB { get; set; }

    [Reactive] public partial string LastId { get; set; }

    public ColourService()
    {
        ColourA = FromHex("FF5B4272");
        ColourB = FromHex("FF1C1C1E");

        /* Plexamp default
         ColourA = FromHex("FF5B4272");
        ColourB = FromHex("FF6C191E");*/
    }
    public async Task Update(string id, string url)
    {
        if (id != LastId)
        {
            LastId = id;
            GetDominateColours(url);
        }
    }
    private async Task GetDominateColours(string url)
    {
        using WebClient client = new();

        var result = await client.DownloadDataTaskAsync(new Uri(url));
        try
        {
            Stream stream = new MemoryStream(result);

            var image = new Avalonia.Media.Imaging.Bitmap(stream);

            var colors = await DominantColorExtractor.GetDominantColorsAsync(image);
            // colors now contains the top 3 dominant Color objects
            foreach (var color in colors)
            {
                Debug.WriteLine($"Color: R={color.R}, G={color.G}, B={color.B}");
                Console.WriteLine($"Color: R={color.R}, G={color.G}, B={color.B}");
            }
            ColourA = colors[0];
            ColourB = colors[1];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
    private Color FromHex( string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Hex string cannot be null or empty", nameof(hex));

        hex = hex.StartsWith("#") ? hex.Substring(1) : hex;

        if (hex.Length != 6 && hex.Length != 8)
            throw new ArgumentException("Hex string must be 6 or 8 characters long (RRGGBB or AARRGGBB)", nameof(hex));

        if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var value))
            throw new ArgumentException("Invalid hex color format", nameof(hex));

        if (hex.Length == 6)
        {
            // RRGGBB format - full opacity
            byte r = (byte)((value >> 16) & 0xFF);
            byte g = (byte)((value >> 8) & 0xFF);
            byte b = (byte)(value & 0xFF);
            return Color.FromArgb(255, r, g, b);
        }
        else
        {
            // AARRGGBB format
            byte a = (byte)((value >> 24) & 0xFF);
            byte r = (byte)((value >> 16) & 0xFF);
            byte g = (byte)((value >> 8) & 0xFF);
            byte b = (byte)(value & 0xFF);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
