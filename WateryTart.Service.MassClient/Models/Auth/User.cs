using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models.Auth
{
    public class User
    {
        [JsonPropertyName("user_id")] public string? UserId { get; set; }
        [JsonPropertyName("username")]public string? Username { get; set; }
        [JsonPropertyName("display_name")]public string? DisplayName { get; set; }
        [JsonPropertyName("role")] public string? Role { get; set; }
    }
}