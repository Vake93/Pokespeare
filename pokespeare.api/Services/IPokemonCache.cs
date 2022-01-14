namespace Pokespeare.Services;

public interface IPokemonCache
{
    void Set<T>(object key, T value, TimeSpan? expiration = null);

    bool TryGetValue<T>(object key, out T value);
}