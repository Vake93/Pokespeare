using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = true,
};

var jsonConfig = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};

var baseTypes = LoadDataFromCSVFile<BaseType>("data/types.csv")
    .ToDictionary(t => t.Id, t => t.Name);

var allPokemon = LoadDataFromCSVFile<Pokemon>("data/pokemon.csv");

var pokemonDescriptions = LoadDataFromCSVFile<PokemonDescription>("data/pokemon_species_flavor_text.csv")
    .Where(pd => pd.Language == "9")
    .GroupBy(pd => pd.Id)
    .Select(g => g.OrderByDescending(pd => pd.Version).First())
    .ToDictionary(pd => pd.Id, pd => pd.Description);

var pokemonTypes = LoadDataFromCSVFile<PokemonType>("data/pokemon_types.csv")
    .Where(pt => pt.Slot == 1)
    .ToDictionary(pt => pt.PokemonId, pt => pt.TypeId);

foreach (var pokemon in allPokemon)
{
    pokemon.Description = CleanText(pokemonDescriptions[pokemon.SpeciesId]);

    if (pokemonTypes.TryGetValue(pokemon.Id, out var typeId))
    {
        pokemon.Type = baseTypes[typeId];
    }
}

using var pokemonJson = new FileStream("data/pokemon.json", FileMode.Create, FileAccess.Write);
await JsonSerializer.SerializeAsync(pokemonJson, allPokemon, jsonConfig).ConfigureAwait(false);
await pokemonJson.FlushAsync().ConfigureAwait(false);

Console.WriteLine("Done!");

T[] LoadDataFromCSVFile<T>(string path)
{
    using var streamReader = new StreamReader(path);
    using var csvReader = new CsvReader(streamReader, csvConfig);

    var data = csvReader.GetRecords<T>().ToArray();

    return data;
}

string CleanText(string? input)
{
    return !string.IsNullOrEmpty(input) ? Regex.Replace(input, @"\t|\\t|\n|\\n|\r|\\r|\f|\\f", " ") : string.Empty;
}

public class BaseType
{
    [Index(0)]
    public int Id { get; set; }

    [Index(1)]
    public string Name { get; set; } = string.Empty;
}

public class PokemonType
{
    [Index(0)]
    public int PokemonId { get; set; }

    [Index(1)]
    public int TypeId { get; set; }

    [Index(2)]
    public int Slot { get; set; }
}

public class Pokemon
{
    [Index(0)]
    public int Id { get; set; }

    [Index(1)]
    public string Name { get; set; } = string.Empty;

    [Index(2)]
    [JsonIgnore]
    public int SpeciesId { get; set; }

    [Ignore]
    public string Description { get; set; } = string.Empty;

    [Ignore]
    public string Sprite => $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png";

    [Ignore]
    public string? Type { get; set; }

    public override string ToString() => Name;
}

public class PokemonDescription
{
    [Index(0)]
    public int Id { get; set; }

    [Index(1)]
    public string Version { get; set; } = string.Empty;

    [Index(2)]
    public string Language { get; set; } = string.Empty;

    [Index(3)]
    public string Description { get; set; } = string.Empty;

    public override string ToString() => Description;
}