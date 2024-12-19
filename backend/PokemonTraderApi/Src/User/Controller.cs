using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

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
  public async Task<ActionResult<PublicPokemonUser>> GetUserInfo()
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    Debug.Assert(user.UserName is not null, "user should always have a username");

    return new PublicPokemonUser { username = user.UserName, coins = user.coins };
  }

  [HttpGet("coins")]
  [Authorize]
  public async Task<ActionResult<long>> GetCoins()
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    return _repo.GetCoins(user.pokemonUserId);
  }

  [HttpPost("coins/free")]
  [Authorize]
  public async Task<ActionResult<long>> GetFreeCoins()
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    return _repo.UpdateCoins(300, user.pokemonUserId);
  }
}
