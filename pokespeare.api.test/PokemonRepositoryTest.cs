using Microsoft.Extensions.Options;
using Pokespeare.Models;
using Pokespeare.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace pokespeare.api.test;

public class PokemonRepositoryTest
{
    [Fact]
    public async Task FileNotFoundTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/no-file.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);
        var count = await pokemonRepository.GetCountAsync().ConfigureAwait(false);

        Assert.Equal(0, count);
    }

    [Fact]
    public async Task FileLoadTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);
        var count = await pokemonRepository.GetCountAsync().ConfigureAwait(false);
        var pokemon = await pokemonRepository.SearchAsync(string.Empty, count, 0).ConfigureAwait(false);

        Assert.Equal(1118, count);
        Assert.True(pokemon.All(p => !string.IsNullOrEmpty(p.Name)));
        Assert.True(pokemon.All(p => !string.IsNullOrEmpty(p.Description)));
        Assert.True(pokemon.All(p => !string.IsNullOrEmpty(p.Sprite)));
        Assert.True(pokemon.All(p => !string.IsNullOrEmpty(p.Type)));
    }

    [Fact]
    public async Task SearchExactTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);

        var results = await pokemonRepository.SearchAsync("charmander", 1, 0).ConfigureAwait(false);

        Assert.NotNull(results);
        Assert.Single(results);

        var charmander = results.First();

        Assert.Equal("charmander", charmander.Name);
    }

    [Fact]
    public async Task SearchFuzzyTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);

        var results = await pokemonRepository.SearchAsync("chamender", 1, 0).ConfigureAwait(false);

        Assert.NotNull(results);
        Assert.Single(results);

        var charmander = results.First();

        Assert.Equal("charmander", charmander.Name);
    }
}
