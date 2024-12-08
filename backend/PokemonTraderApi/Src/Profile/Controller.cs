using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Profile.Controller;

[Route("[controller]")]
public class ProfileController : Util.MyControllerBase
{
  private readonly IRepository _repo;

  public ProfileController(IRepository repository)
  {
    _repo = repository;
  }

  [HttpGet("{name}")]
  public void Get(string name) { }

  [HttpPost]
  [Authorize]
  public void Update() { }
}
