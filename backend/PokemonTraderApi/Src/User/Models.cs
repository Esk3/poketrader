namespace PokemonTraderApi.User;

public class PokemonUser : Microsoft.AspNetCore.Identity.IdentityUser
{
  public long pokemonUserId { get; set; }
  public string? username { get; set; }
  public long coins { get; set; }

  public PokemonUser() { }
}
