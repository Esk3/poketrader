using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace PokemonTraderApi.Market.Controller;

[Route("[controller]")]
public class MarketController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<PokemonTraderApi.User.PokemonUser> _userManager;

  public MarketController(
      IRepository repository,
      UserManager<PokemonTraderApi.User.PokemonUser> userManager
      )
  {
    _repo = repository;
    _userManager = userManager;
  }

  [HttpGet]
  public ActionResult<List<Listing>> GetAllOpenListings()
  {
    return _repo.GetAllOpenListings();
  }

  [HttpGet("{listingId}")]
  public ActionResult<Listing?> GetListing(long listingId)
  {
    return _repo.GetListing(listingId);
  }

  [HttpGet("user")]
  [Authorize]
  public async Task<ActionResult<List<Listing>>> GetUserListings()
  {
    var user = await _userManager.GetUserAsync(User);
    Debug.Assert(user is not null);
    return _repo.GetUserListings(user);
  }

  [HttpPost("new")]
  [Authorize]
  public async Task<ActionResult<long>> CreateListing(Form.CreateListing form)
  {
    var user = await _userManager.GetUserAsync(User);
    Debug.Assert(user is not null);
    return _repo.CreateListing(form.inventoryId, user);
  }

  [HttpGet("{listingId}/bids")]
  public ActionResult<List<Bid>> GetBidsOnListing(long listingId)
  {
    return _repo.GetBidsOnListing(listingId);
  }

  [HttpPost("{listingId}/bid")]
  [Authorize]
  public async void BidOnListing(Form.BidForm bid, long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    Debug.Assert(_repo.BidOnListing(listingId, bid.amount, user));
  }

  [HttpPost("{listingId}/finish")]
  [Authorize]
  public async void FinishListing(long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    _repo.FinishListing(listingId, user);
  }

  [HttpPost("{listingId}/cancel")]
  [Authorize]
  public async void CancelListing(long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    _repo.CancelListing(listingId, user);
  }
}
