namespace PokemonTraderApi.Shop.Exceptions;

public class ItemNotFound : Exception
{
  public long itemId { get; set; }
  public ItemNotFound(long itemId) : base()
  {
    this.itemId = itemId;
  }
}
