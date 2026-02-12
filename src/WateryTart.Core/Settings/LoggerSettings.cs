using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace WateryTart.Core.Settings;

public class LoggerSettings
{
    [JsonPropertyName("logLevel")]
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    [JsonPropertyName("enableFileLogging")]
    public bool EnableFileLogging { get; set; } = false;

    [JsonPropertyName("logFilePath")]
    public string LogFilePath { get; set; } = string.Empty;

    [JsonPropertyName("maxLogFileSizeBytes")]
    public long MaxLogFileSizeBytes { get; set; } = 10 * 1024 * 1024; // 10 MB
}