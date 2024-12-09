using PokemonTraderApi.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace PokemonTraderApi.Shop;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<ShopItem> GetItems();
  public Task<ShopItem?> GetItem(long itemId);
  public Task<ShopItem?> BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public Task<ShopItem?> SellItem(long inventoryItemId, User.PokemonUser user, SqliteTransaction? transaction = null);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly Inventory.IRepository _inventoryRepo;
  private readonly Pokemon.IRepository _pokemonRepo;
  private readonly Random _random;

  public Repository(AppDbContext context, Inventory.IRepository inventoryRepository, Pokemon.IRepository pokemonRepository)
  {
    _context = context;
    _inventoryRepo = inventoryRepository;
    _pokemonRepo = pokemonRepository;
    _random = new Random();
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
          sender_pokemon_user_id integer,
          reciver_pokemon_user_id integer,
          amount integer,
          item_id integer,
          timestamp timestamp not null default current_timestamp,
          foreign key (reciver_pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (sender_pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (item_id) references card_inventory(inventory_id)
          )
        ");
  }

  public bool Test()
  {
    return true;
  }

  public async Task<ShopItem?> BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    /*bool commit = transaction is null;*/
    bool commit = false;
    if (transaction is null && false)
    {
      _context.GetConnection().Open();
      transaction = _context.GetConnection().BeginTransaction();
    }

    var item = await GetItem(itemId);
    _inventoryRepo.InsertItem(itemId, user, transaction);
    await _context.GetConnection().ExecuteAsync(
        @"insert into transfers (sender_pokemon_user_id, amount) values (@Id, @Amount)",
        new { Id = user.pokemonUserId, Amount = item.cost }
        );
    if (commit) { transaction.Commit(); }
    return null;
  }

  public async Task<ShopItem?> GetItem(long itemId)
  {
    ShopItem? item = await _context.GetConnection().QuerySingleOrDefaultAsync<ShopItem>(
        @"
        select * from shop_items
        where pokemon_id = @Id
        ", new { Id = itemId }
        );
    if (item is null)
    {
      item = await Insert(itemId);
    }
    return item;
  }

  public async Task<ShopItem?> Insert(long itemId)
  {
    var pokemon = await _pokemonRepo.GetById(itemId);
    if (pokemon is null) return null;
    int Cost = _random.Next(60, 180);
    var result = await _context.GetConnection().QuerySingleAsync(
        @"insert into shop_items (pokemon_id, cost)
        values (@Id, @Cost);
        select cast(last_insert_rowid() as int) as id",
        new { Id = pokemon.pokemonId, Cost }
        );
    return await _context.GetConnection().QuerySingleAsync(
        @"select * from shop_items
        where pokemon_id = @Id",
        new { Id = result.id }
        );
  }

  public List<ShopItem> GetItems()
  {
    return _context.GetConnection().Query<ShopItem>(
        @"
        select * from shop_items
        "
        ).ToList();
  }

  public async Task<ShopItem?> SellItem(long inventoryItemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    var inventoryItem = _inventoryRepo.GetItem(inventoryItemId, user);
    _inventoryRepo.DeleteItem(inventoryItemId, user, transaction);
    var item = await GetItem(inventoryItem.PokemonId);
    _context.GetConnection().Execute(
        @"insert into transfers (reciver_pokemon_user_id, amount) values (@Id, @Amount)",
        new { Id = user.pokemonUserId, Amount = item.cost },
        transaction
        );
    return null;
  }

}
