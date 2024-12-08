using PokemonTraderApi.Data;
using Dapper;

namespace PokemonTraderApi.User;

public interface IRepository
{
  public PokemonUser? GetByName(string name);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
    Setup();
  }

  private void Setup()
  {
    _context.GetConnection().Execute(@"
        create table if not exists pokemon_users (
          auth_user_id integer primary key,
          pokemon_user_id not null autoincrement,
          foreign key (auth_user_id) references auth_users(auth_user_id)
          )
        ");
  }

  public PokemonUser? GetByName(string name)
  {
    return _context.GetConnection().QuerySingleOrDefault<PokemonUser>(@"
        select * 
        from pokemon_users pu 
        join auth_users au 
        on au.auth_user_id = pu.auth_user_id
        where au.username = @Username
        ", new { Username = name });
  }

}
