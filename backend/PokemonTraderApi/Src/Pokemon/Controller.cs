using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Pokemon.Controller;

[Route("[controller]")]
public class PokemonController : Util.MyControllerBase
{
  [HttpGet]
  public void GetAll() { }

  [HttpGet("{pokemonId}")]
  public void GetById(long pokemonId) { }

  [HttpGet("{name}")]
  public void GetByName(string name) { }
}
