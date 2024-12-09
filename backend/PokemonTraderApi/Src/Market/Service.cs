using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi.Market;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public List<Listing> GetAllOpenListings();
  public Listing? GetListing(long listingId);
  public List<Listing> GetUserListings(User.PokemonUser user);
  public int CreateListing();
  public void BidOnListing(long listingId, int amount, User.PokemonUser user);
  public void FinishListing(long listingId, User.PokemonUser user);
  public void CancelListing(long listingId, User.PokemonUser user);
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
    _context.GetConnection().Execute(@"
        create table if not exists listings (
          listing_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          create_timestamp timestamp not null default current_timestamp,
          closed_timestamp timestamp,
          cancled bool not null default false,
          foreign key (pokemon_user_id) references pokemon_users(pokemon_user_id)
          )
        ");
    _context.GetConnection().Execute(@"
        create table if not exists listing_bids (
          bid_id integer primary key autoincrement,
          listing_id integer not null,
          amount integer,
          inventory_id integer,
          foreign key (listing_id) references listings(listing_id)
          )
        ");
  }

  public void BidOnListing(long listingId, int amount, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public void CancelListing(long listingId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public int CreateListing()
  {
    throw new NotImplementedException();
  }

  public void FinishListing(long listingId, User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public List<Listing> GetAllOpenListings()
  {
    throw new NotImplementedException();
  }

  public Listing? GetListing(long listingId)
  {
    throw new NotImplementedException();
  }

  public List<Listing> GetUserListings(User.PokemonUser user)
  {
    throw new NotImplementedException();
  }

  public bool Test()
  {
    return true;
  }
}
