using Microsoft.AspNetCore.Identity;
using PokemonTraderApi.Data;

namespace PokemonTraderApi;

public class Program
{
  public static void Main(string[] args)
  {
    string allowAllOrigins = "_myAllowAll";
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddTransient<AppDbContext>(e => new AppDbContext(connectionString));

    builder.Services.AddIdentityCore<IdentityUser>().AddSignInManager();

    builder.Services.AddTransient<IUserStore<IdentityUser>, Data.UserStore>();

    builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
      options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
      options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    }).AddIdentityCookies();


    builder.Services.ConfigureApplicationCookie(options =>
{
  options.Events.OnRedirectToLogin = context =>
  {
    context.Response.StatusCode = 401;
    return Task.CompletedTask;
  };
});

    builder.Services.AddCors(options =>
        {
          options.AddPolicy(name: allowAllOrigins,
            policy =>
            {
              policy.WithOrigins("*");
              policy.WithMethods("*");
              policy.WithHeaders("*");
            });
        });

    builder.Services.AddRepositoriesServicesGroup();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    app.SetupRespositoriesServices();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
      app.TestRepositoriesServices();
    }

    app.UseCors(allowAllOrigins);

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
  }
}
