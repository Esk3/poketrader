namespace PokemonTraderApi.Trades.Exceptions;
public class UsernameNotFound : Exception
{
  public UsernameNotFound(string username) : base(username) { }
}
