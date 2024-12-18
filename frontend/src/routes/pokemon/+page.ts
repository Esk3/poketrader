import { ApiUrl, PokemonUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  const res = await fetch(ApiUrl + "Pokemon");
  const data = await res.json();
  return { pokemon: data }
}
