import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  const res = await fetch("/API/Inventory");
  const inventory = await res.json();
  return { inventory };
}
