namespace PokemonTraderApi.Market.Exceptions;

public class InvalidWinnner : Exception
{
  public InvalidWinnner(string username) : base(username) { }
}

public class CloseListing : Exception
{
  public CloseListing() : base() { }
}
