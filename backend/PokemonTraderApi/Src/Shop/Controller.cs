using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Shop.Controller;

[Route("[controller]")]
public class ShopController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<IdentityUser> _userManger;

  public ShopController(IRepository repository, UserManager<IdentityUser> userManager)
  {
    _repo = repository;
    _userManger = userManager;
  }

  [HttpGet]
  public ActionResult<List<ShopItem>> GetAll()
  {
    return _repo.GetItems();
  }

  [HttpGet("{pokemonId}")]
  public async Task<ActionResult<ShopItem?>> GetById(long pokemonId)
  {
    return await _repo.GetItem(pokemonId);
  }

  [HttpGet("{name}")]
  public ShopItem? GetByName(string name)
  {
    return null;
  }

  [HttpPost("{pokemonId}/buy")]
  [Authorize]
  public async Task<ActionResult<ShopItem?>> Buy(long pokemonId)
  {
    var user = await _userManger.GetUserAsync(User);
    var item = await _repo.BuyItem(pokemonId, new PokemonTraderApi.User.PokemonUser(user));
    return item;
  }

  [HttpPost("{pokemonId}/sell")]
  [Authorize]
  public void Sell(long pokemonId) { }
}
