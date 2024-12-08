using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi.Market;

public interface IRepository
{
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

  private void Setup()
  {
    _context.GetConnection().Execute(@"
        create table if not exists listings (
          listing_id integer primary key autoincrement,
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
}
