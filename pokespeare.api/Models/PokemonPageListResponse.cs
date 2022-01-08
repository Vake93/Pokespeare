namespace Pokespeare.Models;

public class PokemonPageListResponse<T> : PokemonResponse<T>
{
    public PokemonPageListResponse(T data, int totalCount)
        : base(data)
    {
        TotalCount = totalCount;
    }

    public int TotalCount { get; protected set; }
}
