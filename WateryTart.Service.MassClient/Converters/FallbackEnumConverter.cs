using System.Text.Json;
using System.Text.Json.Serialization;

namespace WateryTart.Service.MassClient.Converters;

/// <summary>
/// Generic enum converter that falls back to the first enum value (typically "Unknown")
/// when encountering unrecognized strings. Works with snake_case naming.
/// </summary>
public class FallbackEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return default;
            
            // Try direct parse (handles PascalCase)
            if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var directResult))
                return directResult;
            
            // Try converting snake_case to PascalCase
            var pascalCase = ConvertSnakeCaseToPascalCase(value);
            if (Enum.TryParse<TEnum>(pascalCase, ignoreCase: true, out var pascalResult))
                return pascalResult;
            
            // ✅ Fallback to first enum value (typically "Unknown")
            Console.WriteLine($"Unknown {typeof(TEnum).Name}: '{value}' - defaulting to {default(TEnum)}");
            return default;
        }
        
        if (reader.TokenType == JsonTokenType.Number)
        {
            var intValue = reader.GetInt32();
            if (Enum.IsDefined(typeof(TEnum), intValue))
                return (TEnum)(object)intValue;
        }
        
        return default;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        // Convert to snake_case for output
        var name = value.ToString();
        var snakeCase = ConvertPascalCaseToSnakeCase(name);
        writer.WriteStringValue(snakeCase);
    }

    private static string ConvertSnakeCaseToPascalCase(string snakeCase)
    {
        if (string.IsNullOrEmpty(snakeCase))
            return snakeCase;
        
        var parts = snakeCase.Split('_');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].Length > 0)
            {
                parts[i] = char.ToUpper(parts[i][0]) + 
                          (parts[i].Length > 1 ? parts[i].Substring(1).ToLower() : "");
            }
        }
        return string.Join("", parts);
    }

    private static string ConvertPascalCaseToSnakeCase(string pascalCase)
    {
        if (string.IsNullOrEmpty(pascalCase))
            return pascalCase;
        
        return System.Text.RegularExpressions.Regex
            .Replace(pascalCase, "([a-z])([A-Z])", "$1_$2")
            .ToLower();
    }
}