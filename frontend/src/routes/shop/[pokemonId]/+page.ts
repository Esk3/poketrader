import { ApiUrl } from "$lib";
import type { PageLoad } from "../[pokemonName]/$types";

export const load: PageLoad = async ({ params, fetch }) => {

  const url = ApiUrl + "Pokemon/id/" + params.pokemonId;
  const res = await fetch(url);
  const pokemon = await res.json()
  return { pokemonName: params.pokemonName, pokemon }
}
