using Dapper;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class RepositoriesServiceCollectionExtensions
  {
    public static IServiceCollection AddRepositoriesServicesGroup(
         this IServiceCollection services)
    {
      services.AddTransient<PokemonTraderApi.User.IRepository, PokemonTraderApi.User.Repository>();
      services.AddTransient<PokemonTraderApi.TransferRecord.IRepository, PokemonTraderApi.TransferRecord.Repository>();
      services.AddTransient<PokemonTraderApi.Shop.IRepository, PokemonTraderApi.Shop.Repository>();
      services.AddTransient<PokemonTraderApi.Market.IRepository, PokemonTraderApi.Market.Repository>();
      services.AddTransient<PokemonTraderApi.Inventory.IRepository, PokemonTraderApi.Inventory.Repository>();
      services.AddTransient<PokemonTraderApi.Pokemon.IRepository, PokemonTraderApi.Pokemon.Repository>();
      services.AddTransient<PokemonTraderApi.Profile.IRepository, PokemonTraderApi.Profile.Repository>();
      services.AddTransient<PokemonTraderApi.Trades.IRepository, PokemonTraderApi.Trades.Repository>();

      return services;
    }

    public static IApplicationBuilder TestRepositoriesServices(this IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
        var services = serviceScope.ServiceProvider;

        Debug.Assert(services.GetRequiredService<PokemonTraderApi.User.IRepository>().Test(), "Users repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Shop.IRepository>().Test(), "Shop repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Market.IRepository>().Test(), "Markget repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Inventory.IRepository>().Test(), "Inventory repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Pokemon.IRepository>().Test(), "Pokemon repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Profile.IRepository>().Test(), "Profile repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.TransferRecord.IRepository>().Test(), "Transfer Record repository test failed");
        Debug.Assert(services.GetRequiredService<PokemonTraderApi.Trades.IRepository>().Test(), "Trades repository test failed");
      }
      return app;
    }

    public static IApplicationBuilder SetupRespositoriesServices(this IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
        var services = serviceScope.ServiceProvider;

        services.GetRequiredService<PokemonTraderApi.User.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Profile.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Pokemon.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.TransferRecord.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Shop.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Market.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Inventory.IRepository>().Setup();
        services.GetRequiredService<PokemonTraderApi.Trades.IRepository>().Setup();
      }
      return app;
    }
  }
}
