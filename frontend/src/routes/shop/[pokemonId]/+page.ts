import { ApiUrl, fetchJson } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ params, fetch }) => {

  const pokemonId = params.pokemonId;
  const url = ApiUrl + "Shop/id/" + pokemonId;
  const res = await fetch(url);
  const data = await res.json()
  let pokemon = {
    ...(await fetchJson(data.pokemonUrl, fetch)),
    ...data,
  }
  return { pokemonId, pokemon }
}
