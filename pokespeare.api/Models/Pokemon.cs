namespace Pokespeare.Models;

public class Pokemon : IEquatable<Pokemon>
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Sprite { get; init; } = string.Empty;

    public string Type { get; init; } = string.Empty;

    public bool Equals(Pokemon? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj) => Equals(obj as Pokemon);

    public override int GetHashCode() => HashCode.Combine(Id, Name, Description);
}

public record PokemonListItem(int Id, string Name)
{
    public PokemonListItem(Pokemon pokemon)
        : this(pokemon.Id, pokemon.Name)
    {
    }
}

public record PokemonItem(int Id, string Name, string Description, string Sprite, string Type, bool Translated)
{
    public PokemonItem(Pokemon pokemon, TranslationResult translation)
        : this(pokemon.Id, pokemon.Name, translation.Text ?? pokemon.Description, pokemon.Sprite, pokemon.Type, translation.Success)
    {
    }
}