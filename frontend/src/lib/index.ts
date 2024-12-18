export const ApiUrl = new URL("http://localhost:5172")
export const PokemonUrl = new URL("https://pokeapi.co/api/v2");

export const fetchJson = async (url: string, fetch: any) => {
  const res = await fetch(url);
  if (!res.ok) {
    try {
      const json = await res.json();
      throw { res, json };
    } catch (error) {
      throw res;
    }
  }
  const json = await res.json();
  return json;
}
