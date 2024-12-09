using PokemonTraderApi.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace PokemonTraderApi.Inventory;
public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<Item> GetAllItems(User.PokemonUser user, SqliteTransaction? transaction = null);
  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public Item? GetItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public long InsertItem(long pokemonId, User.PokemonUser user, SqliteTransaction? transaction = null);
  public void DeleteItem(long inventoryId, User.PokemonUser user, SqliteTransaction? transaction = null);
}
public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
    Setup();
  }
  public void Setup()
  {
    _context.GetConnection().Open();
    _context.GetConnection().Execute(@"
        create table if not exists card_inventory (
          inventory_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          pokemon_id integer not null,
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id)
          )
        ");
  }

  public Item? GetItem(long itemId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    return _context.GetConnection()
      .QuerySingleOrDefault<Item>("select * from card_inventory where inventory_id = @ItemId and pokemon_user_id = @UserId",
          new { ItemId = itemId, UserId = user.pokemonUserId },
          transaction);
  }

  public List<Item> GetAllItems(User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    return _context.GetConnection()
      .Query<Item>("select * from card_inventory where pokemon_user_id = @UserId",
          new { UserId = user.pokemonUserId },
          transaction)
      .ToList();
    throw new NotImplementedException();
  }

  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    throw new NotImplementedException();
  }

  public bool Test()
  {
    using (var t = _context.GetConnection().BeginTransaction())
    {
      GetItem(1, new User.PokemonUser { }, t);
      t.Rollback();
    }
    return true;
  }

  public long InsertItem(long pokemonId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    var res = _context.GetConnection().QuerySingle(
        @"insert into card_inventory (pokemon_user_id, pokemon_id) values (@UserId, @PokemonId);
          select cast(last_insert_rowid() as int) as id",
        new { UserId = user.pokemonUserId, PokemonId = pokemonId },
        transaction
        );
    var id = res.id;
    return id;
  }

  public void DeleteItem(long inventoryId, User.PokemonUser user, SqliteTransaction? transaction = null)
  {
    _context.GetConnection().Execute(
        @"delete from card_inventory where inventory_id = @Id",
        new { Id = inventoryId },
        transaction
        );
  }
}
