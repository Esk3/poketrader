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

  [HttpGet]
  [AllowAnonymous]
  public ActionResult<string> Index()
  {
    return "hello index";
  }

  [HttpGet("user")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> GetUser()
  {
    var user = User;
    Console.WriteLine(user);
    var TheUser = await _userManager.GetUserAsync(user);
    Console.WriteLine(TheUser);
    return null;
  }

  [HttpPost("signin")]
  [AllowAnonymous]
  public async Task<ActionResult<string>> SignIn()
  {
    var usre = new IdentityUser { UserName = "abc" };
    await _userManager.CreateAsync(usre);
    await _signinManager.SignInAsync(usre, true);
    return null;
  }
}
