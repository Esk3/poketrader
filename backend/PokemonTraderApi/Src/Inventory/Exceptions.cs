namespace PokemonTraderApi.Inventory.Exceptions;

public class ItemNotFound : Exception
{
  public ItemNotFound(long itemId) : base(itemId.ToString()) { }
}
