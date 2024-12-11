import { ApiUrl } from "$lib";

export const load = async ({ fetch, url }) => {
  const grouped = url.searchParams.get("grouped") == "false" ? false : true;
  const endpoint = ApiUrl + "Inventory/info" + (grouped ? "/grouped" : "");
  const res = await fetch(endpoint);
  const inventory = await res.json();
  return { inventory };
}
