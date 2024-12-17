namespace PokemonTraderApi.Market.Form;

public class CreateListing
{
  public int inventoryId { get; set; }
}

public class BidForm
{
  public long inventoryId { get; set; }
}

public class FinishListing
{
  public required string winnerUsername { get; set; }
}
