import { ApiUrl } from "$lib";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch, url }) => {
  const res = await fetch("/API/Inventory");
  let inventory = await res.json();
  inventory = inventory.map(async url => {
    const res = await fetch(url);
    const item = await res.json();
    return item;
  });
  console.log(inventory);
  return { inventory };
}
