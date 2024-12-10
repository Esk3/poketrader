using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.User.Controller;

[Route("[controller]")]
public class UserController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<PokemonUser> _userManager;

  public UserController(IRepository repository, UserManager<PokemonUser> userManager)
  {
    _repo = repository;
    _userManager = userManager;
  }

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<PokemonUser>> GetUserInfo()
  {
    var user = await _userManager.GetUserAsync(User);
    user.coins = _repo.GetCoins(user.pokemonUserId);
    return user;
  }

  [HttpGet("coins")]
  [Authorize]
  public async Task<ActionResult<long>> GetCoins()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.GetCoins(user.pokemonUserId);
  }

  [HttpPost("coins/free")]
  [Authorize]
  public async Task<ActionResult<long>> GetFreeCoins()
  {
    var user = await _userManager.GetUserAsync(User);
    return _repo.UpdateCoins(300, user.pokemonUserId);
  }
}
