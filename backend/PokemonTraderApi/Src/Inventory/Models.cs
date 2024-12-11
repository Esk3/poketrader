namespace PokemonTraderApi.Inventory;

public class Item
{

  public int inventoryId { get; set; }
  public int PokemonId { get; set; }
  public int? count { get; set; }
}


public class InventoryInfo(List<Item> items, List<Pokemon.PokemonSprite> pokemon)
{
  public List<Item> items { get; set; } = items;
  public List<Pokemon.PokemonSprite> pokemon { get; set; } = pokemon;
}
