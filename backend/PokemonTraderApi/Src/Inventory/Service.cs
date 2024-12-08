using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi.Inventory;
public interface IRepository
{
  public List<Item> GetAllItems(User.PokemonUser user);
  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user);
  public Item? GetItem(long itemId, User.PokemonUser user);
}
public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
    Setup();
  }
  private void Setup()
  {
    _context.GetConnection().Execute(@"
        create table if not exists card_inventory (
          inventory_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          pokemon_id integer not null,
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id)
          )
        ");
  }

  public Item? GetItem(long itemId, User.PokemonUser user)
  {
    return _context.GetConnection().QuerySingleOrDefault<Item>("select * from card_inventory where inventory_id = @ItemId and pokemon_user_id = @UserId", new { ItemId = itemId, UserId = user.pokemonUserId });
  }

  public List<Item> GetAllItems(User.PokemonUser user)
  {
    return _context.GetConnection()
      .Query<Item>("select * from card_inventory where pokemon_user_id = @UserId", new { UserId = user.pokemonUserId })
      .ToList();
    throw new NotImplementedException();
  }

  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }
}
