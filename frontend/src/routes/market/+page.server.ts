import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
  const res = await fetch("/API/Market/info");
  let listings = await res.json();
  return {
    listings,
  };
};
