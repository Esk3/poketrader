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
  private readonly LinkGenerator _linkGenerator;

  public InventoryController(
      IRepository repository,
      User.IRepository userRepository,
      UserManager<User.PokemonUser> userManager,
      Pokemon.IRepository pokemonRepository,
      LinkGenerator linkGenerator
      )
  {
    _repo = repository;
    _userRepo = userRepository;
    _userManager = userManager;
    _pokemonRepo = pokemonRepository;
    _linkGenerator = linkGenerator;
  }

  [HttpGet]
  public async Task<ActionResult<List<string>>> GetInventoryView()
  {
    var user = await _userManager.GetUserAsync(User);
    var items = _repo.GetItems(user);
    var Urls = items
      .Select(item => _linkGenerator.GetUriByAction(
            HttpContext,
            nameof(GetItem),
            "inventory",
            new { itemId = item.inventoryId }
            ) ?? throw new InvalidOperationException("error generating URL")
        ).ToList();
    return Urls;
  }

  [HttpGet("all")]
  public async Task<ActionResult<List<string>>> GetAllItems()
  {
    var user = await _userManager.GetUserAsync(User);
    var items = _repo.GetAllItems(user);
    var Urls = items
      .Select(item => _linkGenerator.GetUriByAction(
            HttpContext,
            nameof(GetItem),
            "inventory",
            new { itemId = item.inventoryId }
            ) ?? throw new InvalidOperationException("error generating URL")
        ).ToList();
    return Urls;
  }

  [HttpGet("{itemId}")]
  [AllowAnonymous]
  public async Task<ActionResult<ItemView>> GetItem(long itemId)

  {
    var item = _repo.GetPublicItem(itemId);
    var pokemon = await _pokemonRepo.GetById(item.PokemonId);
    var url = _linkGenerator.GetUriByAction(HttpContext,
        nameof(Pokemon.Controller.PokemonController.GetByName),
        "pokemon",
        new { name = pokemon?.name }
        ) ?? throw new InvalidOperationException("error generating URL");
    return new ItemView
    {
      id = item.inventoryId,
      pokemonId = item.PokemonId,
      name = pokemon?.name ?? "",
      spriteUrl = pokemon?.spriteUrl,
      pokemonurl = url
    };
  }

}
