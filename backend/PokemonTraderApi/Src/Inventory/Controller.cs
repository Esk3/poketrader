using Microsoft.AspNetCore.Authorization;
using PokemonTraderApi.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Inventory.Controller;

[Route("[controller]")]
[Authorize]
public class InventoryController : MyControllerBase
{
  private readonly IRepository _repo;
  private readonly User.IRepository _userRepo;
  readonly UserManager<User.PokemonUser> _userManager;

  public InventoryController(IRepository repository, User.IRepository userRepository, UserManager<User.PokemonUser> userManager)
  {
    _repo = repository;
    _userRepo = userRepository;
    _userManager = userManager;
  }

  [HttpGet]
  public async Task<ActionResult<List<Item>>> GetInventory()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetAllItems(user);
  }

  [HttpGet("{typeId}")]
  public async Task<ActionResult<List<Item>>> GetItemsOfType(long typeId)
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetItemsOfType(typeId, user);
  }

  [HttpGet("item/{itemId}")]
  public async Task<ActionResult<Item?>> GetItem(long itemId)
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetItem(itemId, user);
  }
}
