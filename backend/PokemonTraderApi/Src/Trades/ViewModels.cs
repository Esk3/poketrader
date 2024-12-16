namespace PokemonTraderApi.Trades.ViewModels;

public class TradesView
{
  public List<TradeView>? trades { get; set; }
  public int totalTrades { get; set; }
  public string? next { get; set; }
  public string? previous { get; set; }
}

public class TradeView
{
  public int tradeId { get; set; }
  public string? username1 { get; set; }
  public string? username2 { get; set; }
  public string startTimestamp { get; set; }
  public string? endTimestamp { get; set; }
  public bool cancled { get; set; }
  public string? details { get; set; }

  public TradeView(Trade trade, string user1, string user2)
  {
    tradeId = trade.tradeId;
    username1 = user1;
    username2 = username2;
    startTimestamp = trade.startTimestamp;
    endTimestamp = trade.endTimestamp;
    cancled = trade.cancled;
    details = "";
  }
}

public class TradeDetailsView
{
  public TradeView? trade { get; set; }
  public List<string>? user1ItemsUrls { get; set; }
  public List<string>? user2ItemsUrls { get; set; }
}
