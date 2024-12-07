using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


    // Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddTransient<AppDbContext>(e => new AppDbContext(connectionString));

    builder.Services.AddIdentityCore<IdentityUser>().AddSignInManager();
    /*builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders();*/

    builder.Services.AddTransient<IUserStore<IdentityUser>, Data.UserStore>();
    /*builder.Services.AddTransient<DapperUsersTable>();*/

    builder.Services.AddAuthentication().AddBearerToken();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    /*app.UseHttpsRedirection();*/

    app.UseAuthorization();


    app.MapControllers();

    app.Run();
  }
}
