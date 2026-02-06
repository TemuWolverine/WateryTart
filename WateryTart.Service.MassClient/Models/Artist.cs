using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models;

public class Artist : MediaItemBase
{
    [JsonPropertyName("available")]
    public bool Available { get; set; }
}
