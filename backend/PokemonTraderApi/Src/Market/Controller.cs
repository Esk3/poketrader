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

  [HttpGet("{listingId}/view")]
  public async Task<ActionResult<ListingView>> GetListingView(long listingId)
  {
    var listing = _repo.GetListing(listingId);
    var listingUser = await _userManager.FindByIdAsync(listing.pokemonUserId.ToString());
    var itemViewUrl = "/API/Inventory/item/" + listing.inventoryId + "/view";
    // TODO: limit 1
    var bids = _repo.GetUserBidsOnListing(listingId);
    var maxBid = bids.FirstOrDefault();
    int maxBidValue = 0;
    if (maxBid is not null) { maxBidValue = maxBid.totalValue; }
    return new ListingView
    {
      id = listingId,
      username = listingUser.UserName,
      itemViewUrl = itemViewUrl,
      createTimestamp = listing.createTimestamp,
      closedTimestamp = listing.closedTimestamp,
      cancled = listing.cancled,
      maxBidValue = maxBidValue
    };
  }

  string url(string id) => "/API/Inventory/item/" + id + "/view";

  [HttpGet("{listingId}/bids/view")]
  public async Task<ActionResult<List<UserBidsView>>> GetBidsView(long listingId)
  {
    var queryBids = _repo.GetUserBidsOnListing(listingId);
    var bids = queryBids.Select(bid =>
    {
      var bidView = new UserBidsView
      {
        username = bid.username,
        totalValue = bid.totalValue,
        itemUrls = bid.itemIds?.Split(",").Select(id => url(id)).ToList()
      };
      if (bidView.itemUrls is null) { bidView.itemUrls = new List<string>(); }
      return bidView;
    }).ToList();
    return bids;
  }

  [HttpGet("info")]
  public ActionResult<List<ListingInfo>> GetOpenListingsInfo()
  {
    return _repo.GetOpenListingsInfo();
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
  public async Task BidOnListing(Form.BidForm bid, long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    _repo.BidOnListing(listingId, bid.inventoryId, user);
  }

  [HttpPost("{listingId}/finish")]
  [Authorize]
  public async Task<ActionResult<string>> FinishListing(Form.FinishListing form, long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    await _repo.FinishListing(listingId, form.winnerUsername, user);
    return "ok";
  }

  [HttpPost("{listingId}/cancel")]
  [Authorize]
  public async Task CancelListing(long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    _repo.CancelListing(listingId, user);
  }
}
