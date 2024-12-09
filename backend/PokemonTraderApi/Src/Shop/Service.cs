using PokemonTraderApi.Data;

namespace PokemonTraderApi.Shop;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<ShopItem> GetItems();
  public ShopItem? GetItem(long itemId);
  public ShopItem? BuyItem(long itemId, User.PokemonUser user);
  public ShopItem? SellItem(long inventoryItemId, User.PokemonUser user);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }

  public ShopItem? BuyItem(long itemId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public ShopItem? GetItem(long itemId)
  {
    throw new NotImplementedException();
  }

  public List<ShopItem> GetItems()
  {
    throw new NotImplementedException();
  }

  public ShopItem? SellItem(long inventoryItemId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public void Setup()
  {
  }

  public bool Test()
  {
    return true;
  }
}
