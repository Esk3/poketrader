using PokemonTraderApi.Data;

namespace PokemonTraderApi.Profile;

public interface IRepository
{
  public void Setup();
  public bool Test();
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }

  public void Setup()
  {

  }

  public bool Test()
  {
    return true;
  }
}
