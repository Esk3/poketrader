using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Shop.Controller;

[Route("[controller]")]
public class ShopController : Util.MyControllerBase
{
  private readonly IRepository _repo;

  public ShopController(IRepository repository)
  {
    _repo = repository;
  }

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
