using Pokespeare.Models;

namespace Pokespeare.Services;

public interface IPokemonRepository
{
    Task<int> GetCountAsync();

    Task<Pokemon?> GetAsync(int id);

    Task<IEnumerable<Pokemon>> GetAsync(int take, int skip);

    Task<IEnumerable<Pokemon>> SearchAsync(string searchTerm, int take, int skip);
}