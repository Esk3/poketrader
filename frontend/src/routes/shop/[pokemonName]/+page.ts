import type { PageLoad } from "./$types";

export const load: PageLoad = ({ params }) => {
  return { pokemonName: params.pokemonName }
}
