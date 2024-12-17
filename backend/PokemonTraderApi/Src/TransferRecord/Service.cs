using Dapper;
using System.Diagnostics;

namespace PokemonTraderApi.TransferRecord;
public interface IRepository
{
  public void Setup();
  public bool Test();
  public long RecordTransfer(long? reciverId, long? senderId, int? amount, long? itemId);
  public void RecordTransfers(long? reciverId, long? senderId, List<long> itemIds);
}

public class Repository : IRepository
{
  private readonly Data.AppDbContext _context;

  public Repository(Data.AppDbContext databaseContext)
  {
    _context = databaseContext;
  }

  public long RecordTransfer(long? reciverId, long? senderId, int? amount, long? ItemId)
  {
    Debug.Assert(amount is not null || ItemId is not null);
    Debug.Assert(!(amount is not null && ItemId is not null));
    Debug.Assert(amount > 0 || amount is null);
    Debug.Assert(ItemId > 0);
    return _context.GetConnection().QuerySingle(
        @"insert into transfers (reciver_pokemon_user_id, sender_pokemon_user_id, amount, item_id) values (@Reciver, @Sender, @Amount, @ItemId);
        select cast(last_insert_rowid() as int) as id",
        new { Reciver = reciverId, Sender = senderId, Amount = amount, ItemId }
        ).id;
  }

  public void RecordTransfers(long? Reciver, long? Sender, List<long> itemIds)
  {
    foreach (long itemId in itemIds)
    {
      _context.GetConnection().Execute(
          @"insert into transfers
    (reciver_pokemon_user_id, sender_pokemon_user_id, item_id)
     values
     (@Reciver, @Sender, @Item)",
     new { Reciver, Sender, Item = itemId }
          );
    }
  }

  public void Setup()
  {
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
}
