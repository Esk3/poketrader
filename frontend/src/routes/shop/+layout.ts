
import { ApiUrl } from "$lib";

export const load = async ({ fetch }: any) => {
  try {
    const url = ApiUrl + "Shop/pokemon";
    const res = await fetch(url);
    const data = await res.json();
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
