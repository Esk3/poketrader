namespace PokemonTraderApi.Middleware;
public class ErrorHandler
{
  private readonly RequestDelegate _next;

  public ErrorHandler(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Inventory.Exceptions.ItemNotFound id)
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      await context.Response.WriteAsJsonAsync(new { Message = "item with id: [" + id.Message + "] not found" });
    }

    catch (Market.Exceptions.CloseListing)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await context.Response.WriteAsJsonAsync(new { Message = "error closing listing, it was either already previously closed or you do not have premission to close it" });
    }
    catch (Market.Exceptions.InvalidWinnner e)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await context.Response.WriteAsJsonAsync(new { Message = e.Message + ", is not a valid winner username" });
    }
    catch (Market.Exceptions.ListingCreatorNotFound err)
    {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsJsonAsync(new { Message = "corruped listing. creator not found", id = err.listingId });
    }
    catch (User.Exceptions.NotEnoughCoins)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await context.Response.WriteAsJsonAsync(new { Message = "not enough coins" });
    }
    catch (Trades.Exceptions.UsernameNotFound err)
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      await context.Response.WriteAsJsonAsync(new { Message = "username not found:" + err.Message });

    }
    catch (Trades.Exceptions.CorruptedTrade err)
    {
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await context.Response.WriteAsJsonAsync(new { Message = "Trade corruped", id = err.tradeId });
    }
    catch (Trades.Exceptions.TradeAlreadyClosed err)
    {
      context.Response.StatusCode = StatusCodes.Status400BadRequest;
      await context.Response.WriteAsJsonAsync(new { Message = "Trade already closed", id = err.tradeId });
    }
    catch (Trades.Exceptions.TradeNotFound err)
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      await context.Response.WriteAsJsonAsync(new { Message = "Trade not found", id = err.tradeId });
    }

    catch (Exception ex)
    {
      Console.WriteLine($"Unhandled Exception: {ex}");
      /*throw ex;*/
    }
  }
}
