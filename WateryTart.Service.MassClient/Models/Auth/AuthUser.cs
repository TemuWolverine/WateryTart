using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models.Auth;

public class AuthUser
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("access_token")] public string? AccessToken { get; set; }
    [JsonPropertyName("error")] public string? Error { get; set; }
    [JsonPropertyName("user")] public User? User { get; set; }

}