using Microsoft.Extensions.Caching.Memory;

namespace Pokespeare.Services;

public class PokemonCache : IPokemonCache
{
    private readonly IMemoryCache _memoryCache;

    public PokemonCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set<T>(object key, T value, TimeSpan? expiration = null)
    {
        if (expiration.HasValue)
        {
            _memoryCache.Set(key, value, expiration.Value);
        }
        else
        {
            _memoryCache.Set(key, value);
        }
    }

    public bool TryGetValue<T>(object key, out T value) => _memoryCache.TryGetValue<T>(key, out value);
}
