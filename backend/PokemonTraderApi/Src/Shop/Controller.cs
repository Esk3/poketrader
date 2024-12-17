using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Shop.Controller;

[Route("[controller]")]
public class ShopController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<PokemonTraderApi.User.PokemonUser> _userManger;
  private readonly LinkGenerator _linkGenerator;

  public ShopController(
      IRepository repository,
      UserManager<PokemonTraderApi.User.PokemonUser> userManager,
      LinkGenerator linkGenerator
      )
  {
    _repo = repository;
    _userManger = userManager;
    _linkGenerator = linkGenerator;
  }

  [HttpGet]
  public ActionResult<List<ShopItem>> GetAll()
  {
    var items = _repo.GetItems();
    items.ForEach(item =>
    {
      item.pokemonUrl = _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(Pokemon.Controller.PokemonController.GetById),
          "pokemon",
          new { pokemonId = item.pokemonId }
          ) ?? throw new InvalidOperationException("unable to generate URL");
    });
    return items;
  }

  /*[HttpGet("{pokemonId}")]*/
  /*public async Task<ActionResult<ShopItem?>> GetById(long pokemonId)*/
  /*{*/
  /*  return await _repo.GetItem(pokemonId);*/
  /*}*/
  /**/
  /*[HttpGet("{name}")]*/
  /*public ShopItem? GetByName(string name)*/
  /*{*/
  /*  return null;*/
  /*}*/

  [HttpPost("{pokemonId}/buy")]
  [Authorize]
  public async Task<ActionResult<ShopItem?>> Buy(long pokemonId)
  {
    var user = await _userManger.GetUserAsync(User);
    var item = await _repo.BuyItem(pokemonId, user);
    return item;
  }

  [HttpPost("{pokemonId}/sell")]
  [Authorize]
  public void Sell(long pokemonId) { }
}
