using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Converters;

namespace WateryTart.Service.MassClient.Models.Enums;

[JsonConverter(typeof(FallbackEnumConverter<PlayMode>))]
public enum PlayMode
{
    Unknown,
    Play,
    Replace,
    Next,
    ReplaceNext,
    Add
}