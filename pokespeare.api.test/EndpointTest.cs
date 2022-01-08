using AutoFixture;
using Microsoft.Extensions.Options;
using Pokespeare;
using Pokespeare.Models;
using Pokespeare.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace pokespeare.api.test;

public class EndpointTest
{
    [Fact]
    public async Task ListPokemonTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);

        var response = await Endpoints.ListPokemonAsync(pokemonRepository, null, 10, 0).ConfigureAwait(false);

        var statusCode = response.GetOkObjectResultStatusCode();
        Assert.Equal(200, statusCode);

        var okResult = response.GetOkObjectResultValue<PokemonPageListResponse<IEnumerable<PokemonListItem>>>();
        Assert.NotNull(okResult);
    }

    [Fact]
    public async Task SearchPokemonTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);

        var response = await Endpoints.ListPokemonAsync(pokemonRepository, "charmander", 1, 0).ConfigureAwait(false);

        var statusCode = response.GetOkObjectResultStatusCode();
        Assert.Equal(200, statusCode);

        var okResult = response.GetOkObjectResultValue<PokemonPageListResponse<IEnumerable<PokemonListItem>>>();
        Assert.NotNull(okResult);
        var charmander = Assert.Single(okResult!.Data);

        Assert.Equal("charmander", charmander.Name);
    }

    [Fact]
    public async Task FuzzySearchPokemonTest()
    {
        var logger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
        });

        var pokemonRepository = new PokemonRepository(configuration, logger);

        var response = await Endpoints.ListPokemonAsync(pokemonRepository, "chamender", 1, 0).ConfigureAwait(false);

        var statusCode = response.GetOkObjectResultStatusCode();
        Assert.Equal(200, statusCode);

        var okResult = response.GetOkObjectResultValue<PokemonPageListResponse<IEnumerable<PokemonListItem>>>();
        Assert.NotNull(okResult);
        var charmander = Assert.Single(okResult!.Data);

        Assert.Equal("charmander", charmander.Name);
    }

    [Fact]
    public async Task GetPokemonWithTranslationTest()
    {
        var fixture = new Fixture();

        var repositoryLogger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
            TranslatorUrl = "http://foo/translate",
            TranslationBreakMinutes = 5,
            TranslationCacheMinutes = 5,
        });

        var pokemonRepository = new PokemonRepository(configuration, repositoryLogger);

        var translationResponse = fixture.Create<TranslationResponse>();

        var httpClientFactory = TestHelpers.CreateTestHttpClientFactory(HttpStatusCode.OK, translationResponse);
        var Translatorlogger = TestHelpers.CreateTestLogger<TranslatorService>();
        var memoryCache = TestHelpers.CreateTestMemoryCache();

        var translatorService = new TranslatorService(configuration, httpClientFactory, Translatorlogger, memoryCache);

        var response = await Endpoints.GetPokemonAsync(pokemonRepository, translatorService, 1).ConfigureAwait(false);

        var statusCode = response.GetOkObjectResultStatusCode();
        Assert.Equal(200, statusCode);

        var okResult = response.GetOkObjectResultValue<PokemonResponse<PokemonItem>>();
        Assert.NotNull(okResult);

        Assert.Equal("bulbasaur", okResult!.Data.Name);
        Assert.Equal(translationResponse.Contents.Translated, okResult!.Data.Description);
        Assert.NotEmpty(okResult!.Data.Sprite);
        Assert.True(okResult!.Data.Translated);
    }

    [Fact]
    public async Task GetPokemonFailedTranslationTest()
    {
        var fixture = new Fixture();

        var repositoryLogger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
            TranslatorUrl = "http://foo/translate",
            TranslationBreakMinutes = 5,
            TranslationCacheMinutes = 5,
        });

        var pokemonRepository = new PokemonRepository(configuration, repositoryLogger);

        var translationResponse = fixture.Create<TranslationResponse>();

        var httpClientFactory = TestHelpers.CreateTestHttpClientFactory(HttpStatusCode.TooManyRequests, translationResponse);
        var Translatorlogger = TestHelpers.CreateTestLogger<TranslatorService>();
        var memoryCache = TestHelpers.CreateTestMemoryCache();

        var translatorService = new TranslatorService(configuration, httpClientFactory, Translatorlogger, memoryCache);

        var response = await Endpoints.GetPokemonAsync(pokemonRepository, translatorService, 1).ConfigureAwait(false);

        var statusCode = response.GetOkObjectResultStatusCode();
        Assert.Equal(200, statusCode);

        var okResult = response.GetOkObjectResultValue<PokemonResponse<PokemonItem>>();
        Assert.NotNull(okResult);

        Assert.Equal("bulbasaur", okResult!.Data.Name);
        Assert.NotEmpty(okResult!.Data.Description);
        Assert.NotEmpty(okResult!.Data.Sprite);
        Assert.False(okResult!.Data.Translated);
    }

    [Fact]
    public async Task GetPokemonInvalidId()
    {
        var fixture = new Fixture();

        var repositoryLogger = TestHelpers.CreateTestLogger<PokemonRepository>();
        var configuration = Options.Create(new Configuration
        {
            PokemonFilePath = "Data/pokemon.json",
            TranslatorUrl = "http://foo/translate",
            TranslationBreakMinutes = 5,
            TranslationCacheMinutes = 5,
        });

        var pokemonRepository = new PokemonRepository(configuration, repositoryLogger);

        var translationResponse = fixture.Create<TranslationResponse>();

        var httpClientFactory = TestHelpers.CreateTestHttpClientFactory(HttpStatusCode.OK, translationResponse);
        var Translatorlogger = TestHelpers.CreateTestLogger<TranslatorService>();
        var memoryCache = TestHelpers.CreateTestMemoryCache();

        var translatorService = new TranslatorService(configuration, httpClientFactory, Translatorlogger, memoryCache);

        var response = await Endpoints.GetPokemonAsync(pokemonRepository, translatorService, 4096).ConfigureAwait(false);

        var statusCode = response.GetNotFoundResultStatusCode();
        Assert.Equal(404, statusCode);
    }
}
