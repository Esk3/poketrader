import { ApiUrl, PokemonUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  try {
    const res = await fetch(ApiUrl + "Pokemon");
    const data: [PokemonName] = await res.json();
    console.log(data)
    return { pokemon: data }
  }
  catch (err) {
    console.log(err)
  }
  return {}
}

//export interface PokemonQueryResult {
//  count: number,
//  next: string | null,
//  previous: string | null,
//  results: [PokemonName]
//};

export interface PokemonName {
  pokemonId: number,
  name: string,
  spriteUrl: string
}
