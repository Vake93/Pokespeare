using Pokespeare.Models;

namespace Pokespeare.Services;

public interface ITranslatorService
{
    Task<TranslationResult> TranslateAsync(string text, CancellationToken cancellationToken = default);
}