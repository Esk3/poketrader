using PokemonTraderApi.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace PokemonTraderApi.Inventory;
public interface IRepository
{
  public void Setup();
  public bool Test();

  public List<Item> GetAllItems(User.PokemonUser user);
  public List<Item> GetGroupedItems(User.PokemonUser user);

  public InventoryInfo GetInventoryInfo(User.PokemonUser user);
  public InventoryInfo GetGroupedInventoryInfo(User.PokemonUser user);

  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user);
  public Item? GetItem(long itemId, User.PokemonUser user);
  public Item? GetPublicItem(long itemId);

  public long InsertItem(long pokemonId, User.PokemonUser user);
  public void DeleteItem(long inventoryId, User.PokemonUser user);

  public void MoveItem(long itemId, User.PokemonUser sender, long reciverId);
  public void MoveItems(List<long> itemIds, User.PokemonUser sender, long reciverId);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly TransferRecord.IRepository _transferRecord;

  public Repository(AppDbContext context, TransferRecord.IRepository transferRecord)
  {
    _context = context;
    _transferRecord = transferRecord;
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

  public Item? GetItem(long itemId, User.PokemonUser user)
  {
    return _context.GetConnection()
      .QuerySingleOrDefault<Item>("select * from card_inventory where inventory_id = @ItemId and pokemon_user_id = @UserId",
          new { ItemId = itemId, UserId = user.pokemonUserId }
          );
  }

  public Item? GetPublicItem(long itemId)
  {
    return _context.GetConnection()
      .QuerySingleOrDefault<Item>("select * from card_inventory where inventory_id = @ItemId",
          new { ItemId = itemId });
  }

  public List<Item> GetAllItems(User.PokemonUser user)
  {
    return _context.GetConnection()
      .Query<Item>("select * from card_inventory where pokemon_user_id = @UserId",
          new { UserId = user.pokemonUserId })
      .ToList();
  }

  public List<Item> GetItemsOfType(long pokemonId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public bool Test()
  {
    _context.GetConnection().Open();
    using (var t = _context.GetConnection().BeginTransaction())
    {
      GetItem(1, new User.PokemonUser { });
      t.Rollback();
    }
    return true;
  }

  public long InsertItem(long pokemonId, User.PokemonUser user)
  {
    var res = _context.GetConnection().QuerySingle(
        @"insert into card_inventory (pokemon_user_id, pokemon_id) values (@UserId, @PokemonId);
          select cast(last_insert_rowid() as int) as id",
        new { UserId = user.pokemonUserId, PokemonId = pokemonId }
        );
    var id = res.id;
    return id;
  }

  public void DeleteItem(long inventoryId, User.PokemonUser user)
  {
    _context.GetConnection().Execute(
        @"delete from card_inventory where inventory_id = @Id",
        new { Id = inventoryId });
  }

  public InventoryInfo GetInventoryInfo(User.PokemonUser user)
  {
    var items = GetAllItems(user);
    var pokemonIds = items.Select(item => item.PokemonId);
    var pokemon = _context.GetConnection().Query<Pokemon.PokemonSprite>(
        @"select * from pokemon
        where pokemon_id in @pokemonIds",
        new { pokemonIds }
        ).ToList();
    return new InventoryInfo(items, pokemon);
  }

  public InventoryInfo GetGroupedInventoryInfo(User.PokemonUser user)
  {
    var items = GetGroupedItems(user);
    var pokemonIds = items.Select(item => item.PokemonId);
    var pokemon = _context.GetConnection().Query<Pokemon.PokemonSprite>(
        @"select * from pokemon
        where pokemon_id in @pokemonIds",
        new { pokemonIds }
        ).ToList();
    return new InventoryInfo(items, pokemon);
  }

  public List<Item> GetGroupedItems(User.PokemonUser user)
  {
    return _context.GetConnection()
      .Query<Item>("select *, count(pokemon_id) as count from card_inventory where pokemon_user_id = @UserId group by (pokemon_id)",
          new { UserId = user.pokemonUserId }
          )
      .ToList();
  }

  public void MoveItem(long ItemId, User.PokemonUser sender, long ReciverId)
  {
    var item = GetItem(ItemId, sender);

    if (item is null) throw new Exceptions.ItemNotFound(ItemId);
    _transferRecord.RecordTransfer(ReciverId, sender.pokemonUserId, null, ItemId);

    long rowsUpdated = _context.GetConnection().Execute(
          @"update card_inventory set pokemon_user_id = @ReciverId
          where pokemon_user_id = @SenderId
          and inventory_id = @ItemId",
          new { ReciverId, SenderId = sender.pokemonUserId, ItemId }
        );
    Debug.Assert(rowsUpdated == 1);
  }

  public void MoveItems(List<long> Items, User.PokemonUser sender, long ReciverId)
  {
    var items = _context.GetConnection().Query<long>(
        @"select inventory_id from card_inventory
        where inventory_id in @Items
        and pokemon_user_id = @SenderId",
        new { Items, SenderId = sender.pokemonUserId }
        );

    if (items.Count() != Items.Count()) { }

    _transferRecord.RecordTransfers(ReciverId, sender.pokemonUserId, Items);

    long rowsUpdated = _context.GetConnection().Execute(
          @"update card_inventory set pokemon_user_id = @ReciverId
          where pokemon_user_id = @SenderId
          and inventory_id in @Items",
          new { ReciverId, SenderId = sender.pokemonUserId, Items }
        );
    Debug.Assert(rowsUpdated == Items.Count());
  }
}
