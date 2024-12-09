
import { ApiUrl } from "$lib";

export const load = async ({ fetch }: any) => {
  try {
    const res = await fetch("/API/Shop/pokemon");
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
