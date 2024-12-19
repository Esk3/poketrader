import type { Actions } from "./$types";

export const actions = {
  buy: async ({ fetch, request, params }) => {
    const pokemonId = params.pokemonId;
    const res = await fetch("/API/Shop/" + pokemonId + "/buy", { method: "POST" });

    if (!res.ok) {
      try {
        throw {
          res, body: (await res.json())
        };
      } catch (error) {
        throw res;
      }
    }

  }
} satisfies Actions;
