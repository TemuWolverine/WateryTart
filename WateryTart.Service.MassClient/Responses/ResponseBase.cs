using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Responses;

public abstract class ResponseBase<T>
{
    [JsonPropertyName("message_id")]
    public string? message_id { get; set; }

    [JsonPropertyName("partial")]
    public bool Partial { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }
}