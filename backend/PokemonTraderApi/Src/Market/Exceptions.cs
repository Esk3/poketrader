namespace PokemonTraderApi.Market.Exceptions;

public class InvalidWinnner : Exception
{
  public InvalidWinnner(string username) : base(username) { }
}

public class CloseListing : Exception
{
  public CloseListing() : base() { }
}

public class ListingCreatorNotFound : Exception
{
  public long listingId { get; set; }
  public ListingCreatorNotFound(long listingId) : base()
  {
    this.listingId = listingId;
  }
}
