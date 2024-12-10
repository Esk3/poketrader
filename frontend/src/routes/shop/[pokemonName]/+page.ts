import { ApiUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ params, fetch }) => {

  const url = ApiUrl + "Shop/pokemon/id/" + params.pokemonName;
  const res = await fetch(url);
  const pokemon = await res.json()
  return { pokemonName: params.pokemonName, pokemon }
}
