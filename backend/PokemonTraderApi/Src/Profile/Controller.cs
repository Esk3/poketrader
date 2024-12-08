using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Profile.Controller;

[Route("[controller]")]
public class ProfileController : Util.MyControllerBase
{
  [HttpGet("{name}")]
  public void Get(string name) { }

  [HttpPost]
  [Authorize]
  public void Update() { }
}
