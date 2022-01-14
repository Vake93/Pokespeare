using Pokespeare.Models;
using Pokespeare.Services;

namespace Pokespeare.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<Configuration>(configuration.GetSection("General"));

    public static IServiceCollection AddPokemonCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IPokemonCache, PokemonCache>();

        return services;
    }

    public static IServiceCollection AddPokemonRepository(this IServiceCollection services) =>
        services.AddSingleton<IPokemonRepository, PokemonRepository>();

    public static IServiceCollection AddTranslatorService(this IServiceCollection services)
    {
        services.AddSingleton<ITranslatorService, TranslatorService>();
        services.AddHttpClient(nameof(TranslatorService));

        return services;
    }
}
