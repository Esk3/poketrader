using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi.Pokemon;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<PokemonSprite> GetNames();
  public Task<PokemonSprite?> GetById(long pokemonId);
  public PokemonSprite? GetByName(string name);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly PokeApi.IPokeApiClient _pokeApiClient;
  public Repository(AppDbContext context, PokeApi.IPokeApiClient pokeApiClient)
  {
    _context = context;
    _pokeApiClient = pokeApiClient;
  }

  public void Setup()
  {
    _context.GetConnection().Execute(
        @"create table if not exists pokemon (
          pokemon_id integer not null primary key,
          name text not null,
          sprite_url text
          )"
        );
  }

  public bool Test()
  {
    return true;
  }

  public List<PokemonSprite> GetNames()
  {
    return _context.GetConnection().Query<PokemonSprite>(
        @"select * from pokemon"
        ).ToList();
  }

  public async Task<PokemonSprite?> GetById(long pokemonId)
  {
    var pokemon = _context.GetConnection().QuerySingleOrDefault<PokemonSprite>(
        @"select * from pokemon
        where pokemon_id = @Id",
        new { Id = pokemonId }
        );
    if (pokemon is null)
    {
      var Pokemon = await _pokeApiClient.GetById(pokemonId);
      if (Pokemon is not null)
      {
        pokemon = new PokemonSprite { pokemonId = Pokemon.Id, name = Pokemon.Name, spriteUrl = GetSpriteUrl(Pokemon.Id) };
        _context.GetConnection().Execute(
            "insert into pokemon (pokemon_id, name, sprite_url) values (@Id, @Name, @SpriteUrl)",
            new { Id = pokemon.pokemonId, Name = pokemon.name, SpriteUrl = pokemon.spriteUrl }
            );
      }
    }
    return pokemon;
  }
  string GetSpriteUrl(long pokemonId)
  {
    return "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + pokemonId + ".png";
  }

  public PokemonSprite? GetByName(string name)
  {
    return _context.GetConnection()
      .QuerySingleOrDefault(
          @"select * from pokemon
          where name like @Name",
          new { Name = name }
          );
  }
}
