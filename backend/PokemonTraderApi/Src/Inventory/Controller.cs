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
  private readonly Pokemon.IRepository _pokemonRepo;
  readonly UserManager<User.PokemonUser> _userManager;

  public InventoryController(IRepository repository, User.IRepository userRepository, UserManager<User.PokemonUser> userManager, Pokemon.IRepository pokemonRepository)
  {
    _repo = repository;
    _userRepo = userRepository;
    _userManager = userManager;
    _pokemonRepo = pokemonRepository;
  }

  [HttpGet]
  public async Task<ActionResult<List<Item>>> GetInventory()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetAllItems(user);
  }

  [HttpGet("info")]
  public async Task<ActionResult<InventoryInfo>> GetInventoryInfo()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetInventoryInfo(user);
  }

  [HttpGet("info/grouped")]
  public async Task<ActionResult<InventoryInfo>> GetGroupedInventoryInfo()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetGroupedInventoryInfo(user);
  }

  [HttpGet("{typeId}")]
  public async Task<ActionResult<List<Item>>> GetItemsOfType(long typeId)
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetItemsOfType(typeId, user);
  }

  [HttpGet("item/{itemId}")]
  [AllowAnonymous]
  public async Task<ActionResult<Item?>> GetItem(long itemId)
  {
    /*var user = await _userManager.GetUserAsync(User);*/
    return _repo.GetPublicItem(itemId);
  }

  [HttpGet("item/{itemId}/view")]
  [AllowAnonymous]
  public async Task<ActionResult<ItemView>> GetItemView(long itemId)
  {
    var item = _repo.GetPublicItem(itemId);
    var pokemon = await _pokemonRepo.GetById(item.PokemonId);
    return new ItemView
    {
      id = item.inventoryId,
      pokemonId = item.PokemonId,
      name = pokemon.name,
      spriteUrl = pokemon.spriteUrl,
      pokemonurl = "/API/Pokemon/" + pokemon.name,
    };
  }

  [HttpGet("view")]
  public async Task<ActionResult<List<string>>> GetInventoryView()
  {
    var user = await _userManager.GetUserAsync(User);
    var items = _repo.GetAllItems(user);
    var inventoryUrls = items.Select(item => "/API/Inventory/item/" + item.inventoryId + "/view").ToList();
    return inventoryUrls;
  }
}
