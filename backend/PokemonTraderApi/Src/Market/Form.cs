namespace PokemonTraderApi.Market.Form;

public class CreateListing
{
  public int inventoryId { get; set; }
}

public class BidForm
{
  public int? amount { get; set; }
  public long? inventoryId { get; set; }
}
