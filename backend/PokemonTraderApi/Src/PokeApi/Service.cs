namespace PokemonTraderApi.PokeApi;

public interface IPokeApiClient
{
  public Task<Pokemon?> GetById(long pokemonId);
  public Task<Pokemon?> GetByName(string name);
}

public class PokeApiClient : IPokeApiClient
{
  private static readonly string urlBase = "https://pokeapi.co/api/v2";
  private static readonly string ApiBase = "api/v2";
  private static readonly string pokemonEndpoint = "pokemon";
  private static HttpClient sharedClient = new()
  {
    BaseAddress = new Uri("https://pokeapi.co"),
  };

  public async Task<Pokemon?> GetById(long pokemonId)
  {
    var res = await sharedClient.GetAsync(ApiBase + "/" + pokemonEndpoint + "/" + pokemonId);
    /*var text = await res.Content.ReadAsStringAsync();*/
    var pokemon = await res.Content.ReadFromJsonAsync<Pokemon>();
    return pokemon;
  }

  public async Task<Pokemon?> GetByName(string name)
  {
    var res = await sharedClient.GetAsync(ApiBase + pokemonEndpoint + "/" + name);
    var pokemon = await res.Content.ReadFromJsonAsync<Pokemon>();
    return pokemon;
  }
}
