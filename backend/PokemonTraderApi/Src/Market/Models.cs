namespace PokemonTraderApi.Market;

public class Listing
{
  public int listingId { get; set; }
  public int pokemonUserId { get; set; }
  public int inventoryId { get; set; }
  public string? createTimestamp { get; set; }
  public string? closedTimestamp { get; set; }
  public bool cancled { get; set; }
}

public class ListingInfo
{
  public int listingId { get; set; }
  public int pokemonUserId { get; set; }
  public int inventoryId { get; set; }
  public int pokemonId { get; set; }
  public string? pokemonName { get; set; }
  public string? spriteUrl { get; set; }
  public string? createTimestamp { get; set; }
  public string? closedTimestamp { get; set; }
  public bool cancled { get; set; }
}

public class Bid
{
  public int bidId { get; set; }
  public int listinId { get; set; }
  public int pokemonUserId { get; set; }
  public int amount { get; set; }
  public int inventoryId { get; set; }
}

public class RawUserBids
{
  public int bidId { get; set; }
  public int listingId { get; set; }
  public int pokemonUserId { get; set; }
  public int amount { get; set; }
  public int totalValue { get; set; }
  public string? inventoryIds { get; set; }
}

public class UserBids
{
  public int bidId { get; set; }
  public int listingId { get; set; }
  public int pokemonUserId { get; set; }
  public int amount { get; set; }
  public int totalValue { get; set; }
  public List<int> inventoryIds { get; set; }

  public UserBids(RawUserBids bid)
  {
    bidId = bid.bidId;
    listingId = bid.listingId;
    pokemonUserId = bid.pokemonUserId;
    amount = bid.amount;
    totalValue = bid.totalValue;
    if (bid.inventoryIds is not null)
    {
      inventoryIds = bid.inventoryIds.Split(",").Select(s => int.Parse(s)).ToList();
    }
    else
    {
      inventoryIds = new List<int>();
    }

  }
}

public class UserBidsQueryView
{
  public string? username { get; set; }
  public int totalValue { get; set; }
  public string? itemIds { get; set; }
}
