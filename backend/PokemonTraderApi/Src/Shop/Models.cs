namespace PokemonTraderApi.Shop;

public class ShopItem
{
  public int pokemonId { get; set; }
  public string? pokemonUrl { get; set; }
  public int cost { get; set; }
}

public class ShopPokemon
{
  public int pokemonId { get; set; }
  public string name { get; set; }
  public string spriteUrl { get; set; }
  public int cost { get; set; }
}

public class ShopPokemonInfo { }
