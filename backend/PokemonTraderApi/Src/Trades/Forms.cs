namespace PokemonTraderApi.Trades.Forms;
public class AddForm
{
  public required long inventoryId { get; set; }
}

public class RemoveForm
{
  public required long inventoryId { get; set; }
}


public class CreateForm
{
  public required string otherUsername { get; set; }
}
