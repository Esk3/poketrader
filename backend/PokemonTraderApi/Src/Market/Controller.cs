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
  public void GetAllOpenListings() { }

  [HttpGet("{listingId}")]
  public void GetListing(long listingId) { }

  [HttpGet("user")]
  [Authorize]
  public void GetUserListings() { }

  [HttpPost("new")]
  [Authorize]
  public void CreateListing() { }

  [HttpGet("{listingId}/bids")]
  public void GetBidsOnListing(long listingId) { }

  [HttpPost("{listingId}/bid")]
  [Authorize]
  public void BidOnListing(int amount, long listingId) { }

  [HttpPost("{listingId}/finish")]
  [Authorize]
  public void FinishListing(long listingId) { }

  [HttpPost("{listingId}/cancel")]
  [Authorize]
  public void CancelListing(long listingId) { }
}
