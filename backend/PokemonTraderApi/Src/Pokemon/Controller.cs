using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Pokemon.Controller;

[Route("[controller]")]
public class PokemonController : Util.MyControllerBase
{
  private readonly IRepository _repo;

  public PokemonController(IRepository repository)
  {
    _repo = repository;
  }

  [HttpGet]
  public ActionResult<List<PokemonSprite>> GetAll()
  {
    return _repo.GetNames();
  }

  [HttpGet("id/{pokemonId}")]
  public async Task<ActionResult<PokemonSprite?>> GetById(long pokemonId)
  {
    return await _repo.GetById(pokemonId);
  }

  [HttpGet("name/{name}")]
  public async Task<ActionResult<PokemonSprite?>> GetByName(string name)
  {
    return await _repo.GetByName(name);
  }
}
