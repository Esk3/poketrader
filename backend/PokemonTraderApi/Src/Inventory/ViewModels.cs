namespace PokemonTraderApi.Inventory;

public class ItemView
{
  public int inventoryId { get; set; }
  public int PokemonId { get; set; }
  public string? name { get; set; }
  public string? spriteUrl { get; set; }
  public string? pokemonurl { get; set; }
}


public class InventoryView
{
  public List<ItemView>? items { get; set; }
}

public class GroupedInventoryView { }
