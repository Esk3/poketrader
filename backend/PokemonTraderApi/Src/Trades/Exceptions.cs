namespace PokemonTraderApi.Trades.Exceptions;
public class UsernameNotFound : Exception
{
  public UsernameNotFound(string username) : base(username) { }
}

public class TradeNotFound : Exception
{
  public long tradeId { get; set; }
  public TradeNotFound(long tradeId) : base()
  {
    this.tradeId = tradeId;
  }
}

public class TradeAlreadyClosed : Exception
{
  public long tradeId { get; set; }
  public TradeAlreadyClosed(long tradeId) : base()
  {
    this.tradeId = tradeId;
  }
}

public class CorruptedTrade : Exception
{
  public long tradeId { get; set; }
  public CorruptedTrade(long tradeId) : base()
  {
    this.tradeId = tradeId;
  }
}
