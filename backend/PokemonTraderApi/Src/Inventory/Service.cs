using PokemonTraderApi.Data;

namespace PokemonTraderApi.Inventory;
public interface IRepository
{
  public List<Item> GetAllItems(string username);
  public List<Item> GetItemsOfType(long pokemonId, string username);
  public Item? GetItem(long itemId, string username);
}
public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }
  public Item? GetItem(long itemId, string username)
  {
    throw new NotImplementedException();
  }

  public List<Item> GetAllItems(string username)
  {
    throw new NotImplementedException();
  }

  public List<Item> GetItemsOfType(long pokemonId, string username)
  {
    throw new NotImplementedException();
  }
}
