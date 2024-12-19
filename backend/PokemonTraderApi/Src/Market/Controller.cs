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
  private readonly LinkGenerator _linkGenerator;

  public MarketController(
      IRepository repository,
      UserManager<PokemonTraderApi.User.PokemonUser> userManager,
      LinkGenerator linkGenerator
      )
  {
    _repo = repository;
    _userManager = userManager;
    _linkGenerator = linkGenerator;
  }

  [HttpGet]
  public ActionResult<List<string>> GetAllOpenListings()
  {
    var listings = _repo.GetAllOpenListings();
    var urls = listings.Select(listing => _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(GetListing),
          "market",
          new { listingId = listing.listingId }
          ) ?? throw new InvalidOperationException("unable to generate URL")
        ).ToList();
    return urls;
  }

  [HttpGet("{listingId}")]
  public async Task<ActionResult<ListingView>> GetListing(long listingId)
  {
    var listing = _repo.GetListing(listingId);
    if (listing is null) return NotFound("listing not found");
    var listingUser = await _userManager.FindByIdAsync(listing.pokemonUserId.ToString());
    if (listingUser is null) throw new Exceptions.ListingCreatorNotFound(listingId);

    var itemUrl = _linkGenerator.GetUriByAction(
        HttpContext,
        nameof(Inventory.Controller.InventoryController.GetItem),
        "inventory",
        new { itemId = listing.inventoryId }
        ) ?? throw new InvalidOperationException("unable to generate URL");
    // TODO: limit 1
    var bids = _repo.GetUserBidsOnListing(listingId);
    var maxBid = bids.FirstOrDefault();
    int maxBidValue = 0;
    if (maxBid is not null) { maxBidValue = maxBid.totalValue; }
    return new ListingView
    {
      id = listingId,
      username = listingUser.UserName,
      itemUrl = itemUrl,
      createTimestamp = listing.createTimestamp,
      closedTimestamp = listing.closedTimestamp,
      cancled = listing.cancled,
      maxBidValue = maxBidValue
    };
  }

  [HttpGet("{listingId}/bids")]
  public ActionResult<List<UserBidsView>> GetBidsView(long listingId)
  {
    var queryBids = _repo.GetUserBidsOnListing(listingId);
    var bids = queryBids.Select(bid =>
    {
      var bidView = new UserBidsView
      {
        username = bid.username,
        totalValue = bid.totalValue,
        itemUrls = bid.itemIds?.Split(",").Select(id => _linkGenerator.GetUriByAction(
              HttpContext,
              nameof(Inventory.Controller.InventoryController.GetItem),
              "inventory",
              new { itemId = id }
              ) ?? throw new InvalidOperationException("unable to generate URL")).ToList() ?? new List<string>()
      };
      return bidView;
    }).ToList();
    return bids;
  }

  [HttpPost("new")]
  [Authorize]
  public async Task<ActionResult<long>> CreateListing(Form.CreateListing form)
  {
    var user = await _userManager.GetUserAsync(User);
    Debug.Assert(user is not null);
    return _repo.CreateListing(form.inventoryId, user);
  }

  [HttpPost("{listingId}/bid")]
  [Authorize]
  public async Task<ActionResult> BidOnListing(Form.BidForm bid, long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    _repo.BidOnListing(listingId, bid.inventoryId, user);
    return Ok();
  }

  [HttpPost("{listingId}/finish")]
  [Authorize]
  public async Task<ActionResult> FinishListing(Form.FinishListing form, long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    await _repo.FinishListing(listingId, form.winnerUsername, user);
    return Ok();
  }

  [HttpPost("{listingId}/cancel")]
  [Authorize]
  public async Task<ActionResult> CancelListing(long listingId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    _repo.CancelListing(listingId, user);
    return Ok();
  }

  // TODO: get listings made by user
  // TODO: get listings bid on by user
}
