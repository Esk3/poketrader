using PokemonTraderApi.Data;
using Dapper;
using System.Diagnostics;

namespace PokemonTraderApi.Market;

public interface IRepository
{
  public void Setup();
  public bool Test();

  public List<Listing> GetAllOpenListings();
  public Listing? GetListing(long listingId);
  public List<Listing> GetUserListings(User.PokemonUser user);
  public long CreateListing(long inventoryItemId, User.PokemonUser user);
  public bool BidOnListing(long listingId, int amount, User.PokemonUser user);
  public void FinishListing(long listingId, User.PokemonUser user);
  public void CancelListing(long listingId, User.PokemonUser user);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly User.IRepository _userRepo;
  private readonly TransferRecord.IRepository _transferRepo;
  private readonly Inventory.IRepository _inventoryRepo;

  public Repository(
      AppDbContext context,
      User.IRepository userRepository,
      TransferRecord.IRepository transferRepository,
      Inventory.IRepository inventoryRepository
      )
  {
    _context = context;
    _userRepo = userRepository;
    _transferRepo = transferRepository;
    _inventoryRepo = inventoryRepository;
  }

  public void Setup()
  {
    _context.GetConnection().Execute(@"
        create table if not exists listings (
          listing_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          inventory_id integer not null,
          create_timestamp timestamp not null default current_timestamp,
          closed_timestamp timestamp,
          cancled bool not null default false,
          foreign key (inventory_id) references card_inventory(inventory_id),
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id)
          )
        ");
    _context.GetConnection().Execute(@"
        create table if not exists listing_bids (
          bid_id integer primary key autoincrement,
          listing_id integer not null,
          pokemon_user_id integer not null,
          amount integer,
          inventory_id integer,
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (listing_id) references listings(listing_id)
          )
        ");
  }

  public bool Test()
  {
    return true;
  }

  public bool BidOnListing(long listingId, int amount, User.PokemonUser user)
  {
    // TODO: transaction
    _userRepo.TryUpdateCoins(-amount, user.pokemonUserId);
    _context.GetConnection().Execute(
        @"insert into listing_bids
        (listing_id, amount)
        values
        (@ListingId, @Amount)",
        new { ListingId = listingId, Amount = amount }
        );
    return true;
  }

  public void CancelListing(long listingId, User.PokemonUser user)
  {
    var rowsUpdated = _context.GetConnection().Execute(
        @"update listings set cancled = true, closed_timestamp = current_timestamp
        where listing_id = @ListingId and 
        closed_timestamp is null and
        cancled = false and
        pokemon_user_id = @UserId",
        new { ListingId = listingId, UserId = user.pokemonUserId }
        );
    Debug.Assert(rowsUpdated == 1);
  }

  public long CreateListing(long inventoryItemId, User.PokemonUser user)
  {
    var item = _inventoryRepo.GetItem(inventoryItemId, user);
    Debug.Assert(item is not null, "user doesn't have item in inventory");
    var res = _context.GetConnection().QuerySingle(
        @"insert into listings (pokemon_user_id, inventory_id) values (@UserId, @InventoryId);
        select cast(last_insert_rowid() as int) as id",
        new { UserId = user.pokemonUserId, InventoryId = inventoryItemId }
        );
    return res.id;
  }

  public void FinishListing(long listingId, User.PokemonUser user)
  {
    var rowsUpdated = _context.GetConnection().Execute(
        @"update listing set closed_timestamp = current_timestamp
        where listing_id = @ListingId
        and closed_timestamp is null
        and cancled = false
        and pokemon_user_id = @UserId",
        new { ListingId = listingId, UserId = user.pokemonUserId }
        );
    Debug.Assert(rowsUpdated == 1);
  }

  public List<Listing> GetAllOpenListings()
  {
    return _context.GetConnection().Query<Listing>(
        @"select * from listings
        where closed_timestamp is null"
        ).ToList();
  }

  public Listing? GetListing(long listingId)
  {
    return _context.GetConnection().QuerySingleOrDefault(
        @"select * from listings
        where listing_id = @ListingId",
        new { ListingId = listingId }
        );
  }

  public List<Listing> GetUserListings(User.PokemonUser user)
  {
    return _context.GetConnection().Query<Listing>(
        @"select * from listings
        where pokemon_user_id = @UserId",
        new { UserId = user.pokemonUserId }
        ).ToList();
  }

}
