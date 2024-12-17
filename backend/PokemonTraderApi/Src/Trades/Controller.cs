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
  public async Task<TradesView> GetOpenTradeViews()
  {
    var user = await _userManger.GetUserAsync(User);
    var trades = _repo.GetOpenTradeViews(user);
    var view = new TradesView { trades = trades };
    return view;
  }

  [HttpGet("all")]
  public async Task<List<Trade>> GetAllTrades()
  {
    throw new NotImplementedException();
  }

  [HttpGet("{tradeId}")]
  public async Task<TradeDetailsView> GetTradeDetails(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    var trade = _repo.GetTradeDetailsView(tradeId, user);
    var user2 = await _userManger.FindByIdAsync(trade.trade.pokemonUserId2.ToString());
    var tradeView = new TradeView
    {
      id = trade.trade.tradeId,
      username1 = user.UserName,
      username2 = user2.UserName,
      startTimestamp = trade.trade.startTimestamp,
      endTimestamp = trade.trade.endTimestamp,
      cancled = trade.trade.cancled,
    };
    var inventory1 = trade.user1.Select(id => _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(Inventory.Controller.InventoryController.GetItem),
          "inventory",
          new { itemId = id }
          ) ?? throw new InvalidOperationException("unable to generate URL")).ToList();
    var inventory2 = trade.user2.Select(id => _linkGenerator.GetUriByAction(
          HttpContext,
          nameof(Inventory.Controller.InventoryController.GetItem),
          "inventory",
          new { itemId = id }
          ) ?? throw new InvalidOperationException("unable to generate URL")
        ).ToList();
    var view = new TradeDetailsView { trade = tradeView, user1ItemsUrls = inventory1, user2ItemsUrls = inventory2 };
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
  public async Task AddToTrade(long tradeId, Forms.AddForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.AddInventoryItem(form.inventoryId, tradeId, user);
  }

  [HttpPost("{tradeId}/remove")]
  public async Task RemoveFromTrade(long tradeId, Forms.RemoveForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.RemoveInventoryItem(form.inventoryId, tradeId, user);
  }

  [HttpPost("{tradeId}/lockin")]
  public async Task Lockin(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.LockinOffer(tradeId, user);
  }

}
