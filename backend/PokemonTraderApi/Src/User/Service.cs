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

  public long GetCoins(long userId);
  public long SetCoins(int amount, long userId);
  public long UpdateCoins(int change, long userId);
}

public class Repository : IRepository
{
  private readonly AppDbContext _context;
  public Repository(AppDbContext context)
  {
    _context = context;
  }

  public void Setup()
  {
    _context.GetConnection().Open();
    _context.GetConnection().Execute(@"
        create table if not exists pokemon_users (
          auth_user_id integer primary key autoincrement,
          pokemon_user_id integer not null unique,
          coins integer not null default 0,
          foreign key (auth_user_id) references auth_users(auth_user_id)
          )
        ");
  }
  public bool Test()
  {
    _context.GetConnection().Open();
    using (var transaction = _context.GetConnection().BeginTransaction())
    {
      long id = 321456;
      var user = new IdentityUser { Id = id.ToString(), UserName = "DEBUGGING TEST USER" };
      _context.GetConnection().Execute("insert into auth_users (auth_user_id, username) values (@Id, @UserName)",
          user,
          transaction);
      Register(user, transaction);
      var start = GetCoins(id);
      Debug.Assert(start == 0);
      var set = SetCoins(10, id);
      Debug.Assert(set == 10);
      var update = UpdateCoins(20, id);
      Debug.Assert(update == 30);

      Debug.Assert(GetCoins(000) == -1);
      UpdateCoins(20, 000);
      SetCoins(10, 000);

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
    int rowsInserted = _context.GetConnection().Execute(@"
        insert into pokemon_users (auth_user_id, pokemon_user_id) values (@Id, @Id)
        ", new { Id = user.Id }, transaction);
    Debug.Assert(rowsInserted == 1);
  }

  public long GetCoins(long userId)
  {
    var result = _context.GetConnection().QuerySingleOrDefault(
          "select coins from pokemon_users where pokemon_user_id = @Id",
          new { Id = userId }
          );
    if (result is null) return -1;
    return result.coins;
  }

  public long SetCoins(int amount, long userId)
  {
    var result = _context.GetConnection().QuerySingleOrDefault(
       @"update pokemon_users set coins = @Amount where pokemon_user_id = @UserId;
       select coins from pokemon_users where pokemon_user_id = @UserId",
       new { Amount = amount, UserId = userId }
        );
    if (result is null) return -1;
    return result.coins;
  }

  public long UpdateCoins(int change, long userId)
  {
    var result = _context.GetConnection().QuerySingleOrDefault(
        @"update pokemon_users set coins = coins + @Value where pokemon_user_id = @UserId;
        select coins from pokemon_users where pokemon_user_id = @UserId",
        new { Value = change, UserId = userId }
        );
    if (result is null) return -1;
    return result.coins;
  }
}
