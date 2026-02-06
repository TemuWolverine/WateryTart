using System.Text.Json.Serialization;
using WateryTart.Service.MassClient.Converters;

namespace WateryTart.Service.MassClient.Models.Enums;

[JsonConverter(typeof(FallbackEnumConverter<MediaType>))]
public enum MediaType
{
    Unknown,
    Artist,
    Album,
    Track,
    Genre,
    Playlist,
    Radio,
    Podcast,
    PodcastEpisode,
    Audiobook,
    Folder
}