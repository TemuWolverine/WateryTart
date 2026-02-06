using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models
{
    public class Playlist : MediaItemBase
    {
        [JsonPropertyName("owner")]
        public string? Owner { get; set; }
        
        [JsonPropertyName("is_editable")]
        public bool? IsEditable { get; set; }
    }
}