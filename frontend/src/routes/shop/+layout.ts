
import { ApiUrl } from "$lib";

export const load = async ({ fetch }: any) => {
  const url = ApiUrl + "Shop";
  const res = await fetch(url);
  const data = await res.json();
  const fetchPokemon = async url => {
    const res = await fetch(url);
    const pokemon = await res.json();
    return pokemon;
  }
  const pokemon = data.map(p => {
    return {
      pokemon: fetchPokemon(p.pokemonUrl),
      ...p
    }
  });
  return { pokemon }
}
