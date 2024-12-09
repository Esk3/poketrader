namespace PokemonTraderApi.User;

public class PokemonUser
{
  public long pokemonUserId { get; set; }
  public string? username { get; set; }
  public long coins { get; set; }

  public PokemonUser() { }
  public PokemonUser(Microsoft.AspNetCore.Identity.IdentityUser user)
  {
    pokemonUserId = Int32.Parse(user.Id);
    username = user.UserName;
  }
}
