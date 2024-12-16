using PokemonTraderApi.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace PokemonTraderApi.Shop;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<ShopItem> GetItems();
  public List<ShopPokemon> GetPokemon();
  public Task<ShopPokemon?> GetPokemonById(long pokemonId);
  public ShopPokemon? GetPokemonByName(string name);
  public Task<ShopItem?> GetItem(long itemId);
  public Task<ShopItem?> BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public Task<ShopItem?> SellItem(long inventoryItemId, User.PokemonUser user, SqliteTransaction? transaction = null);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly Inventory.IRepository _inventoryRepo;
  private readonly Pokemon.IRepository _pokemonRepo;
  private readonly User.IRepository _usersRepo;
  private readonly TransferRecord.IRepository _transferRepo;
  private readonly Random _random;

  public Repository(
      AppDbContext context,
      Inventory.IRepository inventoryRepository,
      Pokemon.IRepository pokemonRepository,
      User.IRepository usersRepository,
      TransferRecord.IRepository transferRepository
      )
  {
    _context = context;
    _inventoryRepo = inventoryRepository;
    _pokemonRepo = pokemonRepository;
    _random = new Random();
    _usersRepo = usersRepository;
    _transferRepo = transferRepository;
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
  }

  public bool Test()
  {
    return true;
  }

  public async Task<ShopItem?> BuyItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    // TODO: transaction
    /*bool commit = transaction is null;*/
    bool commit = false;
    if (transaction is null && false)
    {
      _context.GetConnection().Open();
      transaction = _context.GetConnection().BeginTransaction();
    }

    var item = await GetItem(itemId);
    var coinsRemaining = _usersRepo.TryUpdateCoins(-item.cost, user.pokemonUserId);
    if (coinsRemaining == -1) return null;
    _inventoryRepo.InsertItem(itemId, user, transaction);
    _transferRepo.RecordTransfer(null, user.pokemonUserId, item.cost, null);
    if (commit) { transaction.Commit(); }
    return null;
  }

  public async Task<ShopItem?> GetItem(long itemId)
  {
    ShopItem? item = await _context.GetConnection().QuerySingleOrDefaultAsync<ShopItem>(
        @"select * from shop_items
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
    return await _context.GetConnection().QuerySingleAsync<ShopItem>(
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
    // TODO: transaction
    var inventoryItem = _inventoryRepo.GetItem(inventoryItemId, user);
    _inventoryRepo.DeleteItem(inventoryItemId, user, transaction);
    var item = await GetItem(inventoryItem.PokemonId);
    _usersRepo.UpdateCoins(item.cost, user.pokemonUserId);
    _transferRepo.RecordTransfer(user.pokemonUserId, null, item.cost, null);
    return null;
  }

  public List<ShopPokemon> GetPokemon()
  {
    return _context.GetConnection().Query<ShopPokemon>(
        @"select * from shop_items si 
          join pokemon p on p.pokemon_id = si.pokemon_id"
        ).ToList();
  }

  public async Task<ShopPokemon?> GetPokemonById(long pokemonId)
  {
    var query = (long Id) =>
    {
      return _context.GetConnection().QuerySingleOrDefault<ShopPokemon>(
          @"select * from shop_items si 
          join pokemon p on p.pokemon_id = si.pokemon_id
          where p.pokemon_id = @Id",
            new { Id }
          );
    };
    var pokemon = query(pokemonId);
    ShopItem? item;
    if (pokemon is null)
    {
      item = await Insert(pokemonId);
      if (item is not null)
      {
        pokemon = query(item.pokemonId);
      }
    }
    return pokemon;
  }

  public ShopPokemon? GetPokemonByName(string name)
  {
    return _context.GetConnection().QuerySingleOrDefault<ShopPokemon>(
        @"select * from shop_items si 
          join pokemon p on p.pokemon_id = si.pokemon_id
          where p.name = @Name",
          new { Name = name }
        );
  }
}
