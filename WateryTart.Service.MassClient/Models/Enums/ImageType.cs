using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Converters;

namespace WateryTart.Service.MassClient.Models.Enums;

[JsonConverter(typeof(FallbackEnumConverter<ImageType>))]
public enum ImageType
{
    Unknown,
    Thumb,
    ThumbHq,
    Landscape,
    Fanart,
    Discart,
    Clearart,
    Logo,
    Banner,
    cutout,
    Other
}