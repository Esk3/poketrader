import { ApiUrl } from "$lib";
import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch, url }) => {
  // TODO "/Inventory/view"
  const grouped = url.searchParams.get("grouped") == "false" ? false : true;
  const endpoint = ApiUrl + "Inventory/info" + (grouped ? "/grouped" : "");
  const res = await fetch(endpoint);
  const inventory = await res.json();
  return { inventory };
}
