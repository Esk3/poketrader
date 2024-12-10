using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Dapper;

namespace PokemonTraderApi.Data;

public class UserStore : IUserStore<IdentityUser>,
  IUserLoginStore<IdentityUser>,
  IUserPasswordStore<IdentityUser>,
                         IDisposable
/*IUserClaimStore<IdentityUser>,*/
/*IUserRoleStore<IdentityUser>,*/
/*IUserSecurityStampStore<IdentityUser>*/
{
  private readonly AppDbContext _context;
  public readonly Func<SqliteConnection> _conn;

  public UserStore(AppDbContext context)
  {
    _context = context;
    _conn = _context.GetConnection;
    Setup();
  }

  public void Setup()
  {
    _conn().Execute(@"
        create table if not exists auth_users (
          auth_user_id integer primary key autoincrement,
          username text not null
          )
        ");
  }

  public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    await _conn().ExecuteAsync(@"
        insert into auth_users (username) values (@UserName)
        ", new { UserName = user.UserName });
    return IdentityResult.Success;
  }

  public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public void Dispose() { }

  public async Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
  {
    var user = await _conn().QuerySingleOrDefaultAsync<IdentityUser>(@"
        select auth_user_id as id, username from auth_users where auth_user_id = @Id
        ", new { Id = userId });
    return user;
    throw new NotImplementedException();
  }

  public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
  {
    var user = await _conn().QuerySingleOrDefaultAsync<IdentityUser>(@"
        select auth_user_id as id, username from auth_users where username like @UserName
        ", new { UserName = normalizedUserName });
    return user;
    throw new NotImplementedException();
  }

  public async Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    return user.UserName;
    throw new NotImplementedException();
  }

  public async Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    return user.Id;
    throw new NotImplementedException();
  }

  public async Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    return user.UserName;
    throw new NotImplementedException();
  }

  public async Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
  {
    /*var res = await _conn().ExecuteAsync(@"*/
    /*    update auth_users set username = @UserName where auth_user_id = @Id*/
    /*    ", new { Id = user.Id, UserName = user.UserName });*/
    return;
    throw new NotImplementedException();
  }

  public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public async Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
