namespace Pokespeare.Models;

public class PokemonResponse<T>
{
    public PokemonResponse(T data)
    {
        Data = data;
    }

    public T Data { get; protected set; }
}
