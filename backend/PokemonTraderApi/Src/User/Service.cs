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
  public void Register(IdentityUser user);

  public long GetCoins(long userId);
  public long SetCoins(int amount, long userId);
  public long UpdateCoins(int change, long userId);
  public long TryUpdateCoins(int change, long userId);
}

public class Repository : IRepository, IUserStore<PokemonUser>, IDisposable
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
      /*long id = 321456;*/
      /*var user = new IdentityUser { Id = id.ToString(), UserName = "DEBUGGING TEST USER" };*/
      /*_context.GetConnection().Execute("insert into auth_users (auth_user_id, username) values (@Id, @UserName)",*/
      /*    user,*/
      /*    transaction);*/
      /*Register(user);*/
      /*var start = GetCoins(id);*/
      /*Debug.Assert(start == 0);*/
      /*var set = SetCoins(10, id);*/
      /*Debug.Assert(set == 10);*/
      /*var update = UpdateCoins(20, id);*/
      /*Debug.Assert(update == 30);*/
      /**/
      /*Debug.Assert(GetCoins(000) == -1);*/
      /*UpdateCoins(20, 000);*/
      /*SetCoins(10, 000);*/
      /**/
      /*Debug.Assert(TryUpdateCoins(-200, id) == -1);*/
      /*Debug.Assert(GetCoins(id) == 30);*/
      /**/
      /*Debug.Assert(TryUpdateCoins(-10, id) == 20);*/
      /*Debug.Assert(GetCoins(id) == 20);*/
      /**/
      /*Debug.Assert(TryUpdateCoins(20, id) == 40);*/
      /*Debug.Assert(GetCoins(id) == 40);*/
      /**/
      /*transaction.Rollback();*/
    }
    return true;
  }

  public void Register(IdentityUser user)
  {
    int rowsInserted = _context.GetConnection().Execute(@"
        insert into pokemon_users (auth_user_id, pokemon_user_id) values (@Id, @Id)
        ", new { Id = user.Id });
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

  public long TryUpdateCoins(int change, long userId)
  {
    var rowsChanged = _context.GetConnection().Execute(
        "update pokemon_users set coins = coins + @Value where coins + @Value >= 0 and pokemon_user_id = @UserId",
        new { Value = change, UserId = userId }
        );
    if (rowsChanged == 0) throw new Exceptions.NotEnoughCoins();

    // TODO: start transaction to rollback if assert fails
    Debug.Assert(rowsChanged == 1, "error updating coins");

    return GetCoins(userId);
  }

  public Task<string> GetUserIdAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<string?> GetUserNameAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task SetUserNameAsync(PokemonUser user, string? userName, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<string?> GetNormalizedUserNameAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task SetNormalizedUserNameAsync(PokemonUser user, string? normalizedName, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityResult> CreateAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityResult> UpdateAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityResult> DeleteAsync(PokemonUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public async Task<PokemonUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
  {
    return await _context.GetConnection().QuerySingleOrDefaultAsync<PokemonUser>(
        @"select * from pokemon_users pu
        join auth_users au 
        on au.auth_user_id = pu.auth_user_id
        where au.auth_user_id = @UserId",
        new { UserId = userId }
        );
  }

  public async Task<PokemonUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
  {
    return await _context.GetConnection().QuerySingleOrDefaultAsync<PokemonUser>(@"
        select * 
        from pokemon_users pu 
        join auth_users au 
        on au.auth_user_id = pu.auth_user_id
        where au.username like @Username
        ",
        new { Username = normalizedUserName }
        );
  }

  public void Dispose()
  {
  }
}
