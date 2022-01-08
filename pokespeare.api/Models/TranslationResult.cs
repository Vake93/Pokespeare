namespace Pokespeare.Models;

public class TranslationResult
{
    public bool Success { get; init; }

    public string? Text { get; init; }

    public string? ErrorMessage { get; init; }
}
