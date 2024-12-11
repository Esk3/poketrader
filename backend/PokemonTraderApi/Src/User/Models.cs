namespace PokemonTraderApi.User;

public class PokemonUser : Microsoft.AspNetCore.Identity.IdentityUser
{
  public long pokemonUserId { get; set; }
  public long coins { get; set; }

  public PokemonUser() { }
}

public class PublicPokemonUser
{
  public required string username { get; set; }
  public required long coins { get; set; }
}
