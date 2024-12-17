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
  public long offerId { get; set; }
  public long tradeId { get; set; }
  public long pokemonUserId { get; set; }
  public required Type type { get; set; }
  public long inventoryId { get; set; }
  public required string timestamp { get; set; }
}

public enum Type
{
  Add,
  Remove,
  Lockin,
}

public class TradeInventoryIds
{
  public Trade? trade;
  public List<long>? user1;
  public List<long>? user2;
}
