import { ApiUrl, PokemonUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  try {
    const res = await fetch(ApiUrl + "Pokemon");
    const data = await res.json();
    console.log(data)
    return { pokemon: data }
  }
  catch (err) {
    console.log(err)
  }
  return {}
}
