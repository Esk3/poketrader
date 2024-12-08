using PokemonTraderApi.Util;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Inventory.Controller;

[Route("[controller]")]
public class InventoryController : MyControllerBase
{
  private readonly IRepository _repo;

  public InventoryController(IRepository repository)
  {
    _repo = repository;
  }

  [HttpGet]
  public ActionResult<List<Item>> GetInventory()
  {
    return _repo.GetAllItems(User.Identity.Name);
  }

  [HttpGet]
  public ActionResult<List<Item>> GetItemsOfType(long typeId)
  {
    return _repo.GetItemsOfType(typeId, User.Identity.Name);
  }

  [HttpGet("{itemId}")]
  public ActionResult<Item?> GetItem(long itemId)
  {
    return _repo.GetItem(itemId, User.Identity.Name);
  }
}
