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
  public ActionResult<List<PokemonName>> GetAll()
  {
    return _repo.GetNames();
  }

  [HttpGet("{pokemonId}")]
  public ActionResult<PokemonName> GetById(long pokemonId)
  {
    return null;
  }

  [HttpGet("{name}")]
  public ActionResult<PokemonName> GetByName(string name)
  {
    return null;
  }
}
