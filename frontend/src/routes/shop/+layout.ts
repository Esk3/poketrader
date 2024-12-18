
import { ApiUrl } from "$lib";

export const load = async ({ fetch }: any) => {
  const url = ApiUrl + "Shop";
  const res = await fetch(url);
  const data = await res.json();
  const pokemon = data.map(p => {
    return {
      pokemon: (async () => {
        const res = await fetch(p.pokemonUrl);
        const pokemon = await res.json();
        return pokemon;
      })(),
      ...p
    }
  })
  return { pokemon }
}
