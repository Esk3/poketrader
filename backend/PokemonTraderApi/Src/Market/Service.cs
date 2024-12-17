using PokemonTraderApi.Data;
using Dapper;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

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

  public long CreateListing(long inventoryItemId, User.PokemonUser user);
  public void BidOnListing(long listingId, long itemId, User.PokemonUser user);
  public Task FinishListing(long listingId, string winnerUsername, User.PokemonUser user);
  public void CancelListing(long listingId, User.PokemonUser user);

  public List<long> GetUsersWithHighestBids(long listingId, long limit = 3);
  public List<UserBidsQueryView> GetUserBidsOnListing(long ListingId);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  private readonly User.IRepository _userRepo;
  private readonly TransferRecord.IRepository _transferRepo;
  private readonly Inventory.IRepository _inventoryRepo;
  private readonly UserManager<User.PokemonUser> _userManager;

  public Repository(
      AppDbContext context,
      User.IRepository userRepository,
      TransferRecord.IRepository transferRepository,
      Inventory.IRepository inventoryRepository,
      UserManager<User.PokemonUser> userManager
      )
  {
    _context = context;
    _userRepo = userRepository;
    _transferRepo = transferRepository;
    _inventoryRepo = inventoryRepository;
    _userManager = userManager;
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
          inventory_id integer not null,
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id),
          foreign key (listing_id) references listings(listing_id)
          )
        ");
  }

  public bool Test()
  {
    return true;
  }

  public void BidOnListing(long listingId, long ItemId, User.PokemonUser user)
  {
    var item = _inventoryRepo.GetItem(ItemId, user);
    if (item is null) throw new Inventory.Exceptions.ItemNotFound(ItemId);

    long rowsInserted = _context.GetConnection().Execute(
        @"insert into listing_bids
        (listing_id, inventory_id, pokemon_user_id)
        values
        (@ListingId, @ItemId, @UserId)",
        new { ListingId = listingId, ItemId, UserId = user.pokemonUserId }
        );

    Debug.Assert(rowsInserted == 1);
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
    if (item is null) throw new Inventory.Exceptions.ItemNotFound(inventoryItemId);
    var res = _context.GetConnection().QuerySingle(
        @"insert into listings (pokemon_user_id, inventory_id) values (@UserId, @InventoryId);
        select cast(last_insert_rowid() as int) as id",
        new { UserId = user.pokemonUserId, InventoryId = inventoryItemId }
        );
    return res.id;
  }

  public async Task FinishListing(long ListingId, string winnerUsername, User.PokemonUser user)
  {
    var winner = await _userManager.FindByNameAsync(winnerUsername);
    if (winner is null) throw new Exceptions.InvalidWinnner(winnerUsername);

    var rowsUpdated = _context.GetConnection().Execute(
        @"update listings set closed_timestamp = current_timestamp
        where listing_id = @ListingId
        and closed_timestamp is null
        and cancled = false
        and pokemon_user_id = @UserId",
        new { ListingId, UserId = user.pokemonUserId }
        );

    if (rowsUpdated != 1) throw new Exceptions.CloseListing();

    var listing = _context.GetConnection().QuerySingle<Listing>(
        @"select * from listings where listing_id = @ListingId",
        new { ListingId }
        );

    var items = _context.GetConnection().Query<long>(
        @"select inventory_id from listing_bids
        where pokemon_user_id = @WinnerId
        and listing_id = @ListingId",
        new { WinnerId = winner.pokemonUserId, ListingId }
        ).ToList();

    _inventoryRepo.MoveItem(listing.inventoryId, user, winner.pokemonUserId);

    _inventoryRepo.MoveItems(items, winner, user.pokemonUserId);
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


  // TODO: remove
  public List<long> GetUsersWithHighestBids(long ListingId, long Limit = 3)
  {
    return _context.GetConnection().Query<long>(
        @"select b.pokemon_user_id from listing_bids b 
          join card_inventory i on i.inventory_id = b.inventory_id 
          join shop_items s on i.pokemon_id = s.pokemon_id 
          where b.listing_id = @ListingId
          group by b.pokemon_user_id 
          order by sum(cost) 
          limit @Limit",
          new { ListingId, Limit }
        ).ToList();
  }

  public List<UserBidsQueryView> GetUserBidsOnListing(long ListingId)
  {
    return _context.GetConnection().Query<UserBidsQueryView>(
    @"select 
      u.username,
      group_concat(i.inventory_id) as item_ids,
      sum(s.cost) as total_value
      from listing_bids b
        join auth_users u on u.auth_user_id = b.pokemon_user_id 
        join card_inventory i on i.inventory_id = b.inventory_id 
        join shop_items s on s.pokemon_id = i.pokemon_id
      where b.listing_id = @ListingId
      group by b.pokemon_user_id
      order by sum(s.cost) desc
      ",
      new { ListingId }
        ).ToList();
  }
}
