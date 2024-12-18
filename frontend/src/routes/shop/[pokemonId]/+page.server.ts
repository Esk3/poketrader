import type { Actions } from "../[pokemonName]/$types";

export const actions = {
  buy: async ({ fetch, request, params }) => {
    const res = await fetch("/API/Shop/" + params.pokemonName + "/buy", { method: "POST" });
  }
} satisfies Actions;
