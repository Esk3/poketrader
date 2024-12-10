import { ApiUrl } from "$lib";
import type { PageLoad } from "./$types";

export const load: PageLoad = async ({ fetch }) => {
  const url = ApiUrl + "Inventory";
  const res = await fetch(url);
  const inventory = await res.json();
  return { inventory };
}
