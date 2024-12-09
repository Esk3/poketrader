using PokemonTraderApi.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace PokemonTraderApi.Shop;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<ShopItem> GetItems();
  public ShopItem? GetItem(long itemId);
  public ShopItem? BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public ShopItem? SellItem(long inventoryItemId, User.PokemonUser user, SqliteTransaction? transaction = null);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly Inventory.IRepository _inventoryRepo;

  public Repository(AppDbContext context, Inventory.IRepository inventoryRepository)
  {
    _context = context;
    _inventoryRepo = inventoryRepository;
  }

  public void Setup()
  {
    _context.GetConnection().Execute(
        @"create table if not exists shop_items (
          pokemon_id integer primary key,
          cost intger not null,
          foreign key (pokemon_id) references pokemon(pokemon_id)
          )"
        );

    _context.GetConnection().Execute(
        @"create table if not exists transfers (
          transfer_id integer primary key autoincrement,
          reciver_pokemon_user_id integer,
          sender_pokemon_user_id integer,
          amount integer,
          item_id integer,
          timestamp timestamp not null default current_timestamp,
          foreign key (reciver_pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (sender_pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (item_id) references inventory(item_id)
          )
        ");
  }

  public bool Test()
  {
    return true;
  }

  public ShopItem? BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    bool commit = transaction is null;
    if (transaction is null)
    {
      _context.GetConnection().Open();
      transaction = _context.GetConnection().BeginTransaction();
    }

    var item = GetItem(itemId);
    _inventoryRepo.InsertItem(itemId, user, transaction);
    _context.GetConnection().Execute(
        @"insert into transfers (sender_pokemon_user_id, amount) values (@Id, @Amount)",
        new { Id = user.pokemonUserId, Amount = item },
        transaction
        );
    if (commit) { transaction.Commit(); }
    return null;
  }

  public ShopItem? GetItem(long itemId)
  {
    return _context.GetConnection().QuerySingleOrDefault(
        @"
        select * from shop_items
        where pokemon_id = @Id
        ", new { Id = itemId }
        );
  }

  public List<ShopItem> GetItems()
  {
    return _context.GetConnection().Query<ShopItem>(
        @"
        select Æ from shop_items
        "
        ).ToList();
  }

  public ShopItem? SellItem(long inventoryItemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    var inventoryItem = _inventoryRepo.GetItem(inventoryItemId, user);
    _inventoryRepo.DeleteItem(inventoryItemId, user, transaction);
    var item = GetItem(inventoryItem.PokemonId);
    _context.GetConnection().Execute(
        @"insert into transfers (reciver_pokemon_user_id, amount) values (@Id, @Amount)",
        new { Id = user.pokemonUserId, Amount = item },
        transaction
        );
    return null;
  }

}
