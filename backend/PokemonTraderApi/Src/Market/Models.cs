namespace PokemonTraderApi.Market;

public class Listing
{
  public int listingId { get; set; }
  public int pokemonUserId { get; set; }
  public int inventoryId { get; set; }
  public string? createdTimestamp { get; set; }
  public string? closedTimestamp { get; set; }
  public bool cancled { get; set; }
}
public class Bid
{
  public int bidId { get; set; }
  public int listinId { get; set; }
  public int amount { get; set; }
  public int inventoryId { get; set; }
}
