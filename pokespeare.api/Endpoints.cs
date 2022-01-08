using Microsoft.AspNetCore.Mvc;
using Pokespeare.Models;
using Pokespeare.Services;

namespace Pokespeare;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapPokemonEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/api/v1/pokemon", ListPokemonAsync)
            .Produces<PokemonPageListResponse<IEnumerable<PokemonListItem>>>(StatusCodes.Status200OK);

        builder
            .MapGet("/api/v1/pokemon/{id:int}", GetPokemonAsync)
            .Produces<PokemonResponse<PokemonItem>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return builder;
    }

    internal static async Task<IResult> ListPokemonAsync(
        [FromServices] IPokemonRepository pokemonRepository,
        [FromQuery] string? searchTerm,
        [FromQuery] int take = 100,
        [FromQuery] int skip = 0)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            var items = await pokemonRepository.GetAsync(take, skip).ConfigureAwait(false);
            var count = await pokemonRepository.GetCountAsync().ConfigureAwait(false);

            var response = new PokemonPageListResponse<IEnumerable<PokemonListItem>>(
                items.Select(p => new PokemonListItem(p)),
                count);

            return Results.Ok(response);
        }
        else
        {
            var items = await pokemonRepository.SearchAsync(searchTerm, take, skip).ConfigureAwait(false);
            var count = items.Count();

            var response = new PokemonPageListResponse<IEnumerable<PokemonListItem>>(
                items.Select(p => new PokemonListItem(p)),
                count);

            return Results.Ok(response);
        }
    }

    internal static async Task<IResult> GetPokemonAsync(
        [FromServices] IPokemonRepository pokemonRepository,
        [FromServices] ITranslatorService translatorService,
        [FromRoute] int id)
    {
        var pokemon = await pokemonRepository.GetAsync(id).ConfigureAwait(false);

        if (pokemon == null)
        {
            return Results.NotFound();
        }

        var translationResult = await translatorService.TranslateAsync(pokemon.Description).ConfigureAwait(false);

        var response = new PokemonItem(pokemon, translationResult);

        return Results.Ok(new PokemonResponse<PokemonItem>(response));
    }
}
