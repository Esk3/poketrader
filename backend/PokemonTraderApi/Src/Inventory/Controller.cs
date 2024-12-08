using Microsoft.AspNetCore.Authorization;
using PokemonTraderApi.Util;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Inventory.Controller;

[Route("[controller]")]
[Authorize]
public class InventoryController : MyControllerBase
{
  private readonly IRepository _repo;
  private readonly User.IRepository _userRepo;

  public InventoryController(IRepository repository, User.IRepository userRepository)
  {
    _repo = repository;
    _userRepo = userRepository;
  }

  [HttpGet]
  public ActionResult<List<Item>> GetInventory()
  {
    var user = _userRepo.GetByName(User.Identity.Name);
    return _repo.GetAllItems(user);
  }

  [HttpGet("{typeId}")]
  public ActionResult<List<Item>> GetItemsOfType(long typeId)
  {
    var user = _userRepo.GetByName(User.Identity.Name);
    return _repo.GetItemsOfType(typeId, user);
  }

  [HttpGet("item/{itemId}")]
  public ActionResult<Item?> GetItem(long itemId)
  {
    var user = _userRepo.GetByName(User.Identity.Name);
    return _repo.GetItem(itemId, user);
  }
}
