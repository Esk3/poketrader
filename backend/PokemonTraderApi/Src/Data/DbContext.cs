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
