import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ params, fetch }) => {

  const res = await fetch("/API/Shop/pokemon/id/" + params.pokemonName);
  const pokemon = await res.json()
  return { pokemonName: params.pokemonName, pokemon }
}
