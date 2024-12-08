using PokemonTraderApi.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Auth.Controller;

[Route("[controller]")]
public class AuthController : MyControllerBase
{
  private readonly UserManager<IdentityUser> _userManager;
  private readonly SignInManager<IdentityUser> _signinManager;
  public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
  {
    _userManager = userManager;
    _signinManager = signInManager;
  }

  [HttpGet("user")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> GetUser()
  {
    var user = User;
    var TheUser = await _userManager.GetUserAsync(user);
    if (TheUser is null) return NotFound("user not found");
    return TheUser.UserName;
  }

  [HttpPost("register")]
  [AllowAnonymous]
  public async void Register(Form.Register data)
  {
    var user = new IdentityUser { UserName = data.UserName };
    await _userManager.CreateAsync(user);
  }

  [HttpPost("signin")]
  [AllowAnonymous]
  public async void SignIn(Form.SignIn data)
  {
    var user = new IdentityUser { UserName = data.UserName };
    await _signinManager.SignInAsync(user, true);
  }

  [HttpPost("signout")]
  public async void SignOut()
  {
    await _signinManager.SignOutAsync();
  }
}
