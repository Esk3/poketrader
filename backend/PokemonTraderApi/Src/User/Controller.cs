using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.User.Controller;

[Route("[controller]")]
public class UserController : Util.MyControllerBase
{
  private readonly IRepository _repo;

  public UserController(IRepository repository)
  {
    _repo = repository;
  }

  [HttpGet]
  [Authorize]
  public ActionResult<string> GetUsername()
  {
    return User.Identity.Name;
  }
}
