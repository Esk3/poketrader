using PokemonTraderApi.Data;

namespace PokemonTraderApi.Pokemon;

public interface IRepository { }

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }
}
