namespace Pokespeare.Models;

public class Configuration
{
    public string PokemonFilePath { get; set; } = string.Empty;

    public string TranslatorUrl { get; set; } = string.Empty;

    public float TranslationCacheMinutes { get; set; }

    public float TranslationBreakMinutes { get; set; }
}
