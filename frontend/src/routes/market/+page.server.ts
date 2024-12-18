import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ fetch }) => {
  const res = await fetch("/API/Market");
  const data = await res.json();
  const listings = data.map(async listing => {
    const res = await fetch(listing);
    const view = await res.json();

    const itemRes = await fetch(view.itemUrl);
    const itemView = await itemRes.json();

    return { item: itemView, ...view };
  });
  return { listings };
};
