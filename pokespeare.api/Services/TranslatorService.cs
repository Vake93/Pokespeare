using Microsoft.Extensions.Options;
using Pokespeare.Extensions;
using Pokespeare.Models;
using Polly;
using System.Text;
using System.Text.Json;

namespace Pokespeare.Services;

public class TranslatorService : ITranslatorService
{
    private readonly IAsyncPolicy<TranslationResponse?> _policy;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Configuration _configuration;
    private readonly IPokemonCache _pokemonCache;
    private readonly ILogger _logger;

    public TranslatorService(
        IOptions<Configuration> configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<TranslatorService> logger,
        IPokemonCache pokemonCache)
    {
        _logger = logger;
        _pokemonCache = pokemonCache;
        _configuration = configuration.Value;
        _httpClientFactory = httpClientFactory;

        var circuitBreaker = Policy<TranslationResponse?>
            .Handle<Exception>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromMinutes(_configuration.TranslationBreakMinutes));

        _policy = Policy<TranslationResponse?>
            .Handle<Exception>()
            .FallbackAsync(default(TranslationResponse))
            .WrapAsync(circuitBreaker);
    }

    public async Task<TranslationResult> TranslateAsync(string text, CancellationToken cancellationToken = default)
    {
        var request = new TranslationRequest(text);
        var key = text.GetHashCode();

        if (_pokemonCache.TryGetValue<TranslationResult>(key, out var translationResult))
        {
            _logger.LogInformation("Using cached translation result");
            return translationResult;
        }

        var response = await _policy.ExecuteAsync(() => GetTranslationAsync(request, cancellationToken)).ConfigureAwait(false);

        if (response == null)
        {
            _logger.LogError("Failed to read translation response");
            return new TranslationResult
            {
                Success = false,
                ErrorMessage = "Failed to read translation response",
            };
        }

        translationResult = new TranslationResult
        {
            Success = true,
            Text = response.Contents.Translated,
        };

        _pokemonCache.Set(key, translationResult, TimeSpan.FromMinutes(_configuration.TranslationCacheMinutes));

        return translationResult;
    }

    private async Task<TranslationResponse?> GetTranslationAsync(TranslationRequest request, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(TranslatorService));

        var data = new StringContent(JsonSerializer.Serialize(request, JsonExtensions.JsonSerializerOption), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(_configuration.TranslatorUrl, data, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TranslationResponse>(JsonExtensions.JsonSerializerOption, cancellationToken).ConfigureAwait(false);
    }

    private record TranslationContent(string Translated, string Translation);

    private record TranslationRequest(string Text);

    private record TranslationResponse(TranslationContent Contents);
}