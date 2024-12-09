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

  [HttpPost("register")]
  [AllowAnonymous]
  public async Task<ActionResult> Register(Form.Register data, User.IRepository pokemonUserRepository)
  {
    var user = new IdentityUser { UserName = data.UserName };
    var result = await _userManager.CreateAsync(user);
    Console.WriteLine(result.Succeeded);
    Console.WriteLine(result.Errors.FirstOrDefault());
    if (!result.Succeeded) return BadRequest("failed to register");
    user = await _userManager.FindByNameAsync(user.UserName);
    pokemonUserRepository.Register(user);
    return Ok();
  }

  [HttpPost("signin")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> SignIn(Form.SignIn data)
  {
    /*var res = await _signinManager.PasswordSignInAsync(user, "abc", true, false);*/
    // TODO: ignores any password. logs in by username only
    var user = await _userManager.FindByNameAsync(data.UserName);
    if (user is null) return BadRequest("invalid login");

    await _signinManager.SignInAsync(user, true);
    return "Ok";
  }

  [HttpPost("signout")]
  public async void SignOut()
  {
    await _signinManager.SignOutAsync();
  }
}
