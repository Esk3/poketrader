import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
  const res = await fetch("/API/Market");
  const listingUrls = await res.json();

  const listings = listingUrls.map(async (url: string) => {
    const res = await fetch(url);
    const listing = await res.json();

    const itemRes = await fetch(listing.itemUrl);
    const item = await itemRes.json();

    return { item, ...listing };
  });

  return { listings };
};
