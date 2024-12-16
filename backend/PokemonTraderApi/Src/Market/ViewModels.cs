namespace PokemonTraderApi.Market;

public class ListingView
{
  public long listingId { get; set; }
  public string? username { get; set; }
  public string? itemViewUrl { get; set; }
  public string? createTimestamp { get; set; }
  public string? closedTimestamp { get; set; }
  public bool cancled { get; set; }
  public int maxBidValue { get; set; }
}

public class ListingDetailsView
{
  public int listingId { get; set; }
  public string? username { get; set; }
  public int itemId { get; set; }
  public string? itemViewUrl { get; set; }
  public string? createTimestamp { get; set; }
  public string? closedTimestamp { get; set; }
  public bool cancled { get; set; }
}

public class BidView
{
  public int bidId { get; set; }
  public int listinId { get; set; }
  public string? username { get; set; }
  public List<string>? itemViewUrls { get; set; }
  public int value { get; set; }
}

public class BidsView
{
  public int listingId { get; set; }
  public List<string>? bidViewUrls { get; set; }
}
