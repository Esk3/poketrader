using PokemonTraderApi.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Trades;

public interface IRepository
{
  public void Setup();
  public bool Test();


  public List<Trade> GetTrades(User.PokemonUser user);
  public List<Trade> GetAllTrades(User.PokemonUser user);

  public List<TradeView> GetOpenTradeViews(User.PokemonUser user);
  public TradeView GetTradeView(long tradeId, User.PokemonUser user);

  public Trade? GetTrade(long tradeId, User.PokemonUser user);

  public Trade CreateTrade(User.PokemonUser user, User.PokemonUser other);
  public void AddInventoryItem(long inventoryId, long tradeId, User.PokemonUser user);
  public void RemoveInventoryItem(long inventoryId, long tradeId, User.PokemonUser user);
  public Task LockinOffer(long tradeId, User.PokemonUser user);
  public void CancelTrade(long tradeId, User.PokemonUser user);

  public List<Offer> GetOffers(long tradeId, User.PokemonUser user);
  public TradeOffers GetTradeOffers(long tradeId, User.PokemonUser user);

  public TradeInventoryIds? GetTradeDetailsView(long tradeId, User.PokemonUser user);

}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly TransferRecord.IRepository _transferRecord;
  private readonly UserManager<User.PokemonUser> _userManager;
  private readonly Inventory.IRepository _inventoryRepo;

  public Repository(
      AppDbContext context,
      TransferRecord.IRepository transferRecordRepository,
      UserManager<User.PokemonUser> userManager,
      Inventory.IRepository inventoryRepository
      )
  {
    _context = context;
    _transferRecord = transferRecordRepository;
    _userManager = userManager;
    _inventoryRepo = inventoryRepository;
  }

  public void Setup()
  {
    _context.GetConnection().Execute(
        @"create table if not exists card_trades (
          trade_id integer primary key autoincrement,
          pokemon_user_id_1 integer not null,
          pokemon_user_id_2 integer not null,
          start_timestamp datetime default current_timestamp not null,
          end_timestamp datetime,
          cancled bool not null default false,
          foreign key (pokemon_user_id_1) references pokemon_users(pokemon_user_id),
          foreign key (pokemon_user_id_2) references pokemon_users(pokemon_user_id)
          )"
        );
    _context.GetConnection().Execute(
        @"create table if not exists card_trades_offers (
            offer_id integer primary key autoincrement,
            trade_id integer not null,
            pokemon_user_id integer not null,
            type text CHECK( type IN ('add','remove','lockin') ) not null,
            inventory_id integer,
            timestamp datetime default current_timestamp not null,
            foreign key (trade_id) references card_trades(trade_id),
            foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id),
            foreign key (inventory_id) references card_inventory(inventory_id)
            )"
        );
  }

  public bool Test()
  {
    return true;
  }

  public void AddInventoryItem(long InventoryId, long TradeId, User.PokemonUser user)
  {
    try
    {

      _context.GetConnection().Execute(
          @"insert into card_trades_offers 
        (trade_id, pokemon_user_id, type, inventory_id)
        values
        (@TradeId, @UserId, 'add', @InventoryId)",
          new { TradeId, InventoryId, UserId = user.pokemonUserId }
          );

    }
    catch (System.Exception err)
    {
      Console.WriteLine(err);
    }
  }

  public void CancelTrade(long tradeId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public Trade CreateTrade(User.PokemonUser user, User.PokemonUser other)
  {
    var result = _context.GetConnection().QuerySingle(
        @"insert into card_trades (pokemon_user_id_1, pokemon_user_id_2) values (@UserId, @OtherUserId);
        select cast (last_insert_rowid() as int) as id;",
        new { UserId = user.pokemonUserId, OtherUserId = other.pokemonUserId }
        );
    return _context.GetConnection().QuerySingle<Trade>(
        "select * from card_trades where trade_id = @Id",
        new { Id = result.id }
        );
  }

  public Trade? GetTrade(long TradeId, User.PokemonUser user)
  {
    return _context.GetConnection().QuerySingleOrDefault<Trade>(
        @"select * from card_trades where 
        trade_id = @TradeId
        and @UserId in (pokemon_user_id_1, pokemon_user_id_2)
        ",
        new { TradeId, UserId = user.pokemonUserId }
        );
  }

  public List<Trade> GetTrades(User.PokemonUser user)
  {
    return _context.GetConnection().Query<Trade>(
        @"select * from card_trades where 
        @UserId in (pokemon_user_id_1, pokemon_user_id_2)
        ",
        new { UserId = user.pokemonUserId }
        ).ToList();
  }

  public async Task LockinOffer(long TradeId, User.PokemonUser user)
  {
    _context.GetConnection().Execute(
        @"insert into card_trades_offers 
        (trade_id, pokemon_user_id, type)
        values
        (@TradeId, @UserId, 'lockin')",
        new { TradeId, UserId = user.pokemonUserId }
        );
    if (tradeIsFinshed(TradeId))
    {
      await finishTrade(TradeId, user);
    }
  }

  bool tradeIsFinshed(long TradeId)
  {
    var last2 = _context.GetConnection().Query<Offer>(
        @"select * from card_trades_offers 
        where trade_id = @TradeId 
        order by timestamp 
        desc limit 2",
        new { TradeId }
        );
    var actions = last2.Take(2)
      .DistinctBy(action => action.pokemonUserId)
      .TakeWhile(action => action.type == Type.Lockin);

    return actions.Count() == 2;
  }
  async Task finishTrade(long TradeId, User.PokemonUser user)
  {
    var trade = GetTrade(TradeId, user);
    if (trade is null) throw new Exceptions.TradeNotFound(TradeId);

    var rowsUpdated = _context.GetConnection().Execute(
        @"update card_trades set end_timestamp = current_timestamp 
          where trade_id = @TradeId
          and @UserId in (pokemon_user_id_1, pokemon_user_id_2)
          and end_timestamp is null",
        new { TradeId, UserId = user.pokemonUserId }
        );

    if (rowsUpdated == 0) throw new Exceptions.TradeAlreadyClosed(TradeId);
    Debug.Assert(rowsUpdated == 1);

    long otherUserId;
    if (trade.pokemonUserId1 == user.pokemonUserId)
    {
      otherUserId = trade.pokemonUserId2;
    }
    else
    {
      otherUserId = trade.pokemonUserId1;
    }

    var otherUser = await _userManager.FindByIdAsync(otherUserId.ToString());

    if (otherUser is null) throw new Exceptions.CorruptedTrade(TradeId);

    var offers = GetOffers(TradeId, user);

    if (offers is null) throw new NotImplementedException("TODO: abort empty trade");

    var (usersOffers, otherUsersOffers) = SplitItemsInOffer(offers, user);

    _inventoryRepo.MoveItems(usersOffers.Select(offer => offer.inventoryId).ToList(), user, otherUserId);

    _inventoryRepo.MoveItems(otherUsersOffers.Select(offer => offer.inventoryId).ToList(), otherUser, user.pokemonUserId);
  }

  public void RemoveInventoryItem(long InventoryId, long TradeId, User.PokemonUser user)
  {
    _context.GetConnection().Execute(
        @"insert into card_trades_offers 
        (trade_id, pokemon_user_id, type, inventory_id)
        values
        (@TradeId, @UserId, 'remove', @InventoryId)",
        new { TradeId, InventoryId, UserId = user.pokemonUserId }
        );
  }

  public List<Offer> GetOffers(long TradeId, User.PokemonUser user)
  {
    return _context.GetConnection().Query<Offer>(
        // TODO userid
        "select * from card_trades_offers where trade_id = @TradeId",
        new { TradeId }
        ).ToList();
  }

  public TradeOffers GetTradeOffers(long tradeId, User.PokemonUser user)
  {
    Trade? trade = GetTrade(tradeId, user);
    List<Offer>? offers = GetOffers(tradeId, user);
    if (offers is null)
    {
      throw new NotImplementedException();
    }
    if (trade is null)
    {
      throw new NotImplementedException();

    }
    return new TradeOffers(trade, offers);
  }

  public TradeInventoryIds? GetTradeDetailsView(long tradeId, User.PokemonUser user)
  {
    var trade = GetTrade(tradeId, user);
    var allOffers = GetOffers(tradeId, user);
    var user1 = GetItemsInOffer(allOffers.Where(offer => offer.pokemonUserId == user.pokemonUserId));
    var user2 = GetItemsInOffer(allOffers.Where(offer => offer.pokemonUserId != user.pokemonUserId));
    return new TradeInventoryIds { trade = trade, user1 = user1, user2 = user2 };
  }

  (List<Offer>, List<Offer>) SplitItemsInOffer(List<Offer> offers, User.PokemonUser user)
  {
    var thisUser = offers.Where(offer => offer.pokemonUserId == user.pokemonUserId).Where(offer => offer.inventoryId > 0).ToList();
    var otherUser = offers.Where(offer => offer.pokemonUserId != user.pokemonUserId).Where(offer => offer.inventoryId > 0).ToList();
    return (thisUser, otherUser);
  }

  List<long> GetItemsInOffer(IEnumerable<Offer> offers)
  {
    var result = new List<long>();
    foreach (var offer in offers)
    {
      if (offer.type == Type.Add)
      {
        result.Add(offer.inventoryId);
      }
      else if (offer.type == Type.Remove)
      {
        result.Remove(offer.inventoryId);
      }
    }
    return result;

  }

  public List<Trade> GetAllTrades(User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public List<TradeView> GetOpenTradeViews(User.PokemonUser user)
  {
    return _context.GetConnection().Query<TradeView>(
        @"select 
          t.trade_id as id,
          u1.username as username1,
          u2.username as username2,
          t.start_timestamp,
          t.end_timestamp,
          t.cancled
        from card_trades t
        join auth_users u1 on u1.auth_user_id = t.pokemon_user_id_1
        join auth_users u2 on u2.auth_user_id = t.pokemon_user_id_2
        where t.end_timestamp is null"
        ).ToList();
  }

  public TradeView GetTradeView(long tradeId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }
}
