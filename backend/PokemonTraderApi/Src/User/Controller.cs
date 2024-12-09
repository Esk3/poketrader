using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.User.Controller;

[Route("[controller]")]
public class UserController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<IdentityUser> _userManager;

  public UserController(IRepository repository, UserManager<IdentityUser> userManager)
  {
    _repo = repository;
    _userManager = userManager;
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PokemonUser>> GetUserInfo()
  {
    var idUser = await _userManager.GetUserAsync(User);
    var user = new PokemonUser(idUser);
    user.coins = _repo.GetCoins(user.pokemonUserId);
    return user;
  }

  [HttpGet("coins")]
  [Authorize]
  public async Task<ActionResult<long>> GetCoins()
  {
    var idUser = await _userManager.GetUserAsync(User);
    var user = new PokemonUser(idUser);
    return _repo.GetCoins(user.pokemonUserId);
  }

  [HttpPost("coins/free")]
  [Authorize]
  public async Task<ActionResult<long>> GetFreeCoins()
  {
    var idUser = await _userManager.GetUserAsync(User);
    var user = new PokemonUser(idUser);
    return _repo.UpdateCoins(300, user.pokemonUserId);
  }
}
