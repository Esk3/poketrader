import type { Actions } from "./$types";

export const actions = {
  buy: async ({ fetch, request, params }) => {
    console.log("here");
    const res = await fetch("/API/Shop/" + params.pokemonName + "/buy", { method: "POST" });
    console.log(res);
  }
} satisfies Actions;
