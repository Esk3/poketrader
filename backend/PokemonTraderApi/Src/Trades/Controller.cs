using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace PokemonTraderApi.Trades.Controller;

[Route("[controller]")]
[Authorize]
public class TradesController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<PokemonTraderApi.User.PokemonUser> _userManger;
  private readonly LinkGenerator _linkGenerator;

  public TradesController(IRepository repository, UserManager<PokemonTraderApi.User.PokemonUser> userManager, LinkGenerator linkGenerator)
  {
    _repo = repository;
    _userManger = userManager;
    _linkGenerator = linkGenerator;
  }

  [HttpGet]
  public async Task<ActionResult<TradesView>> GetOpenTradeViews()
  {
    var user = await _userManger.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    var trades = _repo.GetOpenTradeViews(user);
    var view = new TradesView { trades = trades };
    return view;
  }

  [HttpGet("all")]
  public Task<ActionResult<List<Trade>>> GetAllTrades()
  {
    throw new NotImplementedException();
  }

  [HttpGet("{tradeId}")]
  public async Task<ActionResult<TradeDetailsView>> GetTradeDetails(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");


    var trade = _repo.GetTradeDetailsView(tradeId, user);
    if (trade is null) return NotFound("trade not found");
    if (trade.trade is null) throw new NotImplementedException("err trade.trade not found");

    long otherUserId;
    if (trade.trade.pokemonUserId1 == (user.pokemonUserId))
    {
      otherUserId = trade.trade.pokemonUserId2;
    }
    else
    {
      otherUserId = trade.trade.pokemonUserId1;
    }

    var otherUser = await _userManger.FindByIdAsync(otherUserId.ToString());
    if (otherUser is null) throw new NotImplementedException("trading partner not found");

    var tradeView = new TradeView
    {
      id = trade.trade.tradeId,
      username1 = user.UserName,
      username2 = otherUser.UserName,
      startTimestamp = trade.trade.startTimestamp,
      endTimestamp = trade.trade.endTimestamp,
      cancled = trade.trade.cancled,
    };

    Debug.Assert(trade.user1 is not null);
    Debug.Assert(trade.user2 is not null);

    var userInventory = trade.user1.Select(id => _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(Inventory.Controller.InventoryController.GetItem),
          "inventory",
          new { itemId = id }
          ) ?? throw new InvalidOperationException("unable to generate URL")).ToList();

    var otherUserInventory = trade.user2.Select(id => _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(Inventory.Controller.InventoryController.GetItem),
          "inventory",
          new { itemId = id }
          ) ?? throw new InvalidOperationException("unable to generate URL")
        ).ToList();

    var view = new TradeDetailsView { trade = tradeView, user1ItemsUrls = userInventory, user2ItemsUrls = otherUserInventory };
    return view;
  }

  [HttpPost("create")]
  public async Task<Trade> CreateTrade(Forms.CreateForm otherUsername)
  {
    var user = await _userManger.GetUserAsync(User);
    var other = await _userManger.FindByNameAsync(otherUsername.otherUsername);
    Debug.Assert(user is not null, "user is null");
    if (other is null) throw new Exceptions.UsernameNotFound(otherUsername.otherUsername);
    return _repo.CreateTrade(user, other);
  }

  [HttpPost("{tradeId}/add")]
  public async Task<ActionResult> AddToTrade(long tradeId, Forms.AddForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    _repo.AddInventoryItem(form.inventoryId, tradeId, user);
    return Ok();
  }

  [HttpPost("{tradeId}/remove")]
  public async Task<ActionResult> RemoveFromTrade(long tradeId, Forms.RemoveForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    _repo.RemoveInventoryItem(form.inventoryId, tradeId, user);
    return Ok();
  }

  [HttpPost("{tradeId}/lockin")]
  public async Task<ActionResult> Lockin(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    if (user is null) return Forbid("pokemon user not found");
    await _repo.LockinOffer(tradeId, user);
    return Ok();
  }

}
