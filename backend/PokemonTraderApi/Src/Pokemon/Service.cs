using PokemonTraderApi.Data;

namespace PokemonTraderApi.Pokemon;

public interface IRepository
{
  public List<PokemonName> GetNames();
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }

  public List<PokemonName> GetNames()
  {
    return new List<PokemonName> { new PokemonName { name = "hello" }, new PokemonName { name = "world" } };
  }
}
