using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.Trades.Controller;

[Route("[controller]")]
[Authorize]
public class TradesController : Util.MyControllerBase
{
  private readonly IRepository _repo;
  private readonly UserManager<PokemonTraderApi.User.PokemonUser> _userManger;

  public TradesController(IRepository repository, UserManager<PokemonTraderApi.User.PokemonUser> userManager)
  {
    _repo = repository;
    _userManger = userManager;
  }

  [HttpGet]
  public async Task<List<Trade>> GetTrades()
  {
    var user = await _userManger.GetUserAsync(User);
    return _repo.GetTrades(user);
  }
  [HttpGet("{tradeId}")]
  public async Task<Trade?> GetTrade(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    return _repo.GetTrade(tradeId, user);
  }

  [HttpPost("create")]
  public async Task<Trade> CreateTrade(string otherUsername)
  {
    var user = await _userManger.GetUserAsync(User);
    var other = await _userManger.FindByNameAsync(otherUsername);
    return _repo.CreateTrade(user, other);
  }

  [HttpPost("{tradeId}/add")]
  public async void AddToTrade(long tradeId, Forms.AddForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.AddInventoryItem(form.inventoryId, tradeId, user);
  }

  [HttpPost("{tradeId}/remove")]
  public async void RemoveFromTrade(long tradeId, Forms.RemoveForm form)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.RemoveInventoryItem(form.inventoryId, tradeId, user);
  }

  [HttpPost("{tradeId}/lockin")]
  public async void Lockin(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    _repo.LockinOffer(tradeId, user);
  }

  [HttpGet("{tradeId}/offers")]
  public async Task<List<Offer>> GetOffers(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    return _repo.GetOffers(tradeId, user);
  }

  [HttpGet("{tradeId}/offers/info")]
  public async Task<TradeOffers> GetTradeOffers(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    return _repo.GetTradeOffers(tradeId, user);
  }

  [HttpGet("{tradeId}/details")]
  public async Task<ViewModels.TradeDetailsView> GetTradeDetails(long tradeId)
  {
    var user = await _userManger.GetUserAsync(User);
    var trade = _repo.GetTradeDetailsView(tradeId, user);
    var user2 = await _userManger.FindByIdAsync(trade.trade.pokemonUserId2.ToString());
    var tradeView = new ViewModels.TradeView(trade.trade, user.UserName, user2.UserName);
    var inventory1 = trade.user1ItemsInventoryIds.Select(id => "/API/Inventory/item/" + id).ToList();
    var inventory2 = trade.user2ItemsInventoryIds.Select(id => "/API/Inventory/item/" + id).ToList();
    var view = new ViewModels.TradeDetailsView { trade = tradeView, user1ItemsUrls = inventory1, user2ItemsUrls = inventory2 };
    return view;
  }
}
