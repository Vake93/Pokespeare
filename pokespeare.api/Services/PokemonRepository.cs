using Microsoft.Extensions.Options;
using Pokespeare.Extensions;
using Pokespeare.Models;
using SimMetrics.Net.Metric;
using System.Diagnostics;
using System.Text.Json;

namespace Pokespeare.Services;

public class PokemonRepository : IPokemonRepository
{
    private readonly Configuration _appConfig;
    private readonly Levenstein _levenstein;
    private readonly ILogger _logger;

    public PokemonRepository(
        IOptions<Configuration> appConfig,
        ILogger<PokemonRepository> logger)
    {
        _levenstein = new Levenstein();
        _appConfig = appConfig.Value;
        _logger = logger;
    }

    private Pokemon[]? Pokemon { get; set; }

    public async Task<Pokemon?> GetAsync(int id)
    {
        if (Pokemon == null)
        {
            Pokemon = await LoadPokemonAsync().ConfigureAwait(false);
        }

        return Pokemon.FirstOrDefault(p => p.Id == id);
    }

    public async Task<int> GetCountAsync()
    {
        if (Pokemon == null)
        {
            Pokemon = await LoadPokemonAsync().ConfigureAwait(false);
        }

        return Pokemon.Length;
    }

    public async Task<IEnumerable<Pokemon>> GetAsync(int take, int skip)
    {
        if (Pokemon == null)
        {
            Pokemon = await LoadPokemonAsync().ConfigureAwait(false);
        }

        return Pokemon.Skip(skip).Take(take);
    }

    public async Task<IEnumerable<Pokemon>> SearchAsync(string searchTerm, int take, int skip)
    {
        if (Pokemon == null)
        {
            Pokemon = await LoadPokemonAsync().ConfigureAwait(false);
        }

        return Pokemon.OrderByDescending(p => _levenstein.GetSimilarity(p.Name, searchTerm)).Skip(skip).Take(take);
    }

    private async Task<Pokemon[]> LoadPokemonAsync()
    {
        try
        {
            using var jsonFileStream = new FileStream(_appConfig.PokemonFilePath, FileMode.Open, FileAccess.Read);
            var pokemon = await JsonSerializer.DeserializeAsync<Pokemon[]>(jsonFileStream, JsonExtensions.JsonSerializerOption).ConfigureAwait(false);

            if (pokemon == null)
            {
                _logger.LogWarning("{jsonFile} contains no Pokemon data", _appConfig.PokemonFilePath);
            }

            return pokemon ?? Array.Empty<Pokemon>();
        }
        catch (FileNotFoundException e)
        {
            _logger.LogError(e.Demystify(), "{jsonFile} not found", _appConfig.PokemonFilePath);
            return Array.Empty<Pokemon>();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Demystify(), "Error deserializing {jsonFile}", _appConfig.PokemonFilePath);
            return Array.Empty<Pokemon>();
        }
    }
}
