import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
  const res = await fetch("/API/Market");
  const listings = await res.json();
  console.log(listings);
  return {
    listings
  };
};
