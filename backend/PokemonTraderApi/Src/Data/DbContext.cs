using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Dapper;
namespace PokemonTraderApi.Data;

public class AppDbContext
{
  private readonly string _connectionString;
  private readonly SqliteConnection _connection;
  public AppDbContext(string connectionString)
  {
    _connectionString = connectionString;
    _connection = new SqliteConnection(connectionString);
    Setup();
  }

  public SqliteConnection GetConnection()
  {
    return _connection;
  }

  void Setup()
  {
  }
}
public class UserStore : IUserStore<IdentityUser>,
                         IDisposable
/*IUserClaimStore<IdentityUser>,*/
/*IUserRoleStore<IdentityUser>,*/
/*IUserPasswordStore<IdentityUser>,*/
/*IUserSecurityStampStore<IdentityUser>*/
{
  private readonly AppDbContext _context;
  public UserStore(AppDbContext context)
  {
    _context = context;
    Setup();
  }

  void Setup()
  {

  }

  public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public void Dispose() { }

  public async Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
  {
    return new IdentityUser { UserName = "Todo" };
    throw new NotImplementedException();
  }

  public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
  {
    return new IdentityUser { UserName = normalizedUserName };
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

  public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
  {
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
}
