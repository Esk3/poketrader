
import { PokemonUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  try {
    const res = await fetch(PokemonUrl + "/pokemon");
    const data: PokemonQueryResult = await res.json();
    return { pokemon: data }
  }
  catch (err) {
    console.log(err)
  }
  return {}
}

export interface PokemonQueryResult {
  count: number,
  next: string | null,
  previous: string | null,
  results: [PokemonName]
};

export interface PokemonName {
  name: string,
  url: string
}
