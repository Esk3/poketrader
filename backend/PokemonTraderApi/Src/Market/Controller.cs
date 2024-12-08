using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PokemonTraderApi.Market.Controller;

[Route("[controller]")]
public class MarketController : Util.MyControllerBase
{
  private readonly IRepository _repo;

  public MarketController(IRepository repository)
  {
    _repo = repository;
  }

  [HttpGet]
  public void GetAllOpenTrades() { }

  [HttpGet("{tradeId}")]
  public void GetTrade(long tradeId) { }

  [HttpGet("user")]
  [Authorize]
  public void GetUserTrades() { }

  [HttpPost("new")]
  [Authorize]
  public void CreateTrade() { }

  [HttpPost("{tradeId}/bid")]
  [Authorize]
  public void BidOntrade(int amount, long tradeId) { }

  [HttpPost("{tradeId}/finish")]
  [Authorize]
  public void FinishTrade(long tradeId) { }

  [HttpPost("{tradeId}/cancel")]
  [Authorize]
  public void CancelTrade(long tradeId) { }
}
