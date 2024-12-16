using PokemonTraderApi.Data;
using Dapper;
using System.Diagnostics;

namespace PokemonTraderApi.Market;

public interface IRepository
{
  public void Setup();
  public bool Test();

  public List<Listing> GetAllOpenListings();
  public List<ListingInfo> GetOpenListingsInfo();

  public Listing? GetListing(long listingId);
  public ListingInfo? GetListingInfo(long listingId);

  public List<Listing> GetUserListings(User.PokemonUser user);
  public List<ListingInfo> GetUserListingsInfo(User.PokemonUser user);

  public List<Bid> GetBidsOnListing(long ListingId);
  public List<UserBids> GetGroupSortedBidsOnListing(long listingId);
  public UserBids? GetMaxUserBidOnListing(long listingId);

  public long CreateListing(long inventoryItemId, User.PokemonUser user);
  public bool BidOnListing(long listingId, int amount, User.PokemonUser user);
  public bool BidPokemonOnListing(long listingId, long inventoryId, User.PokemonUser user);
  public void FinishListing(long listingId, User.PokemonUser user);
  public void CancelListing(long listingId, User.PokemonUser user);

  public ListingView GetListingView();
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
        (listing_id, amount, pokemon_user_id)
        values
        (@ListingId, @Amount, @UserId)",
        new { ListingId = listingId, Amount = amount, UserId = user.pokemonUserId }
        );
    return true;
  }

  public bool BidPokemonOnListing(long ListingId, long InventoryId, User.PokemonUser user)
  {
    var item = _inventoryRepo.GetItem(InventoryId, user);
    if (item is null)
    {
      return false;
    }

    var rowsInserted = _context.GetConnection().Execute(
        @"insert into listing_bids
        (listing_id, inventory_id, pokemon_user_id)
        values
        (@ListingId, @InventoryId, @UserId)",
        new { ListingId, InventoryId, UserId = user.pokemonUserId }
        );
    Debug.Assert(rowsInserted == 1);
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
    return _context.GetConnection().QuerySingleOrDefault<Listing>(
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

  public List<Bid> GetBidsOnListing(long ListingId)
  {
    return _context.GetConnection().Query<Bid>(
        @"select * from listing_bids
          where listing_id = @ListingId",
        new { ListingId }
        ).ToList();
  }

  public List<UserBids> GetGroupSortedBidsOnListing(long ListingId)
  {
    // TODO: list of inventory_id's & join on detailed inventory item
    var result = _context.GetConnection().Query<RawUserBids>(
        @"select bid_id, listing_id, pokemon_user_id, sum(amount) as amount, group_concat(inventory_id) as inventory_ids from listing_bids
        where listing_id = @ListingId
        group by (pokemon_user_id)
        order by amount desc",
        new { ListingId }
        );
    return result.Select(row =>
    {
      return new UserBids(row);
    }).ToList();
  }

  public List<ListingInfo> GetOpenListingsInfo()
  {
    return _context.GetConnection().Query<ListingInfo>(
        @"select l.*, p.pokemon_id, p.name as pokemon_name, p.sprite_url from listings l
        join card_inventory i on i.inventory_id = l.inventory_id
        join pokemon p on p.pokemon_id = i.pokemon_id
        where closed_timestamp is null"
        ).ToList();
  }

  public ListingInfo? GetListingInfo(long ListingId)
  {
    return _context.GetConnection().QuerySingleOrDefault<ListingInfo>(
        @"select l.*, p.pokemon_id, p.name as pokemon_name, p.sprite_url from listings l
        join card_inventory i on i.inventory_id = l.inventory_id
        join pokemon p on p.pokemon_id = i.pokemon_id
        where listing_id = @listingId",
        new { ListingId }
        );
  }

  public List<ListingInfo> GetUserListingsInfo(User.PokemonUser user)
  {
    return _context.GetConnection().Query<ListingInfo>(
        @"select l.*, p.pokemon_id, p.name as pokemon_name, p.sprite_url from listings l
        join card_inventory i on i.inventory_id = l.inventory_id
        join pokemon p on p.pokemon_id = i.pokemon_id
        where pokemon_user_id = @UserId",
        new { UserId = user.pokemonUserId }
        ).ToList();
  }

  public UserBids? GetMaxUserBidOnListing(long ListingId)
  {
    // TODO: list of inventory_id's & join on detailed inventory item
    return _context.GetConnection().QuerySingleOrDefault<UserBids>(
        @"select bid_id, listing_id, pokemon_user_id, sum(amount) as amount from listing_bids
        where listing_id = @ListingId
        group by (pokemon_user_id)
        order by amount desc
        limit 1",
        new { ListingId }
        );
  }

  public ListingView GetListingView()
  {
    var listing = GetListing(1);
    var username = "xyz";
    throw new NotImplementedException();
  }
}
