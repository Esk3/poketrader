using PokemonTraderApi.Data;
using System.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace PokemonTraderApi.User;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public PokemonUser? GetByName(string name);
  public void Register(IdentityUser user);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
    Setup();
  }

  public void Setup()
  {
    _context.GetConnection().Execute(@"
        create table if not exists pokemon_users (
          auth_user_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          foreign key (auth_user_id) references auth_users(auth_user_id)
          )
        ");
  }
  public bool Test() { return true; }

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

  public void Register(IdentityUser user)
  {
    Console.WriteLine("here");
    int rowsInserted = _context.GetConnection().Execute(@"
        insert into pokemon_users (auth_user_id, pokemon_user_id) values (@Id, @Id)
        ", new { Id = user.Id });
    Debug.Assert(rowsInserted == 1);
  }
}
