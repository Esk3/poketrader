using PokemonTraderApi.Data;
using System.Diagnostics;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;

namespace PokemonTraderApi.User;

public interface IRepository
{
  public void Setup();
  public bool Test();
  public PokemonUser? GetByName(string name, SqliteTransaction? transaction = null);
  public void Register(IdentityUser user, SqliteTransaction? transaction = null);
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
    _context.GetConnection().Open();
    _context.GetConnection().Execute(@"
        create table if not exists pokemon_users (
          auth_user_id integer primary key autoincrement,
          pokemon_user_id integer not null,
          foreign key (auth_user_id) references auth_users(auth_user_id)
          )
        ");
  }
  public bool Test()
  {
    using (var transaction = _context.GetConnection().BeginTransaction())
    {
      _context.GetConnection().Execute("insert into auth_users (auth_user_id, username) values (321456, 'DEBUGGING TEST USER')",
          new { },
          transaction);
      transaction.Rollback();
    }
    return true;
  }

  public PokemonUser? GetByName(string name, SqliteTransaction? transaction = null)
  {
    return _context.GetConnection().QuerySingleOrDefault<PokemonUser>(@"
        select * 
        from pokemon_users pu 
        join auth_users au 
        on au.auth_user_id = pu.auth_user_id
        where au.username = @Username
        ",
        new { Username = name },
        transaction);
  }

  public void Register(IdentityUser user, SqliteTransaction? transaction = null)
  {
    Console.WriteLine("here");
    int rowsInserted = _context.GetConnection().Execute(@"
        insert into pokemon_users (auth_user_id, pokemon_user_id) values (@Id, @Id)
        ", new { Id = user.Id }, transaction);
    Debug.Assert(rowsInserted == 1);
  }
}
