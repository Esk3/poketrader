using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Shop.Controller;

[Route("[controller]")]
public class ShoptController : Util.MyControllerBase
{
  [HttpGet]
  public void GetAll() { }

  [HttpGet("{pokemonId}")]
  public void GetById(long pokemonId) { }

  [HttpGet("{name}")]
  public void GetByName(string name) { }

  [HttpPost("{pokemonId}/buy")]
  [Authorize]
  public void Buy(long pokemonId) { }

  [HttpPost("{pokemonId}/sell")]
  [Authorize]
  public void Sell(long pokemonId) { }
}
