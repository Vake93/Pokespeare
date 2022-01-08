using System.Text.Encodings.Web;
using System.Text.Json;

namespace Pokespeare.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static JsonSerializerOptions JsonSerializerOption => _jsonSerializerOptions;
}
