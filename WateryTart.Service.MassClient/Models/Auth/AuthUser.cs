using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Models.Auth;

public class AuthUser
{
    [JsonPropertyName("success")] public bool success { get; set; }
    [JsonPropertyName("access_token")] public string? access_token { get; set; }
    [JsonPropertyName("error")] public string? error { get; set; }
    [JsonPropertyName("user")] public User? user { get; set; }

}