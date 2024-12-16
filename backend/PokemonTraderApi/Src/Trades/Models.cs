namespace PokemonTraderApi.Trades;

public class Trade
{
  public int tradeId { get; set; }
  public int pokemonUserId1 { get; set; }
  public int pokemonUserId2 { get; set; }
  public required string startTimestamp { get; set; }
  public string? endTimestamp { get; set; }
  public bool cancled { get; set; }
}

public class TradeDetailsView
{
  public Trade? trade { get; set; }
  public List<int>? user1ItemsInventoryIds { get; set; }
  public List<int>? user2ItemsInventoryIds { get; set; }
}

public class TradeOffers
{
  public Trade trade { get; set; }
  public List<Offer> offers { get; set; }
  public TradeOffers(Trade trade, List<Offer> offers)
  {
    this.trade = trade;
    this.offers = offers;
  }
}

public class Offer
{
  public int offerId { get; set; }
  public int tradeId { get; set; }
  public int pokemonUserId { get; set; }
  public required Type type { get; set; }
  public int inventoryId { get; set; }
  public required string timestamp { get; set; }
}

public enum Type
{
  Add,
  Remove,
  Lockin,
}
