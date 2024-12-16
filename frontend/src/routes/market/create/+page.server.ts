import { ApiUrl } from "$lib";
import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch, }) => {
  const endpoint = ApiUrl + "Inventory/info/grouped";
  const res = await fetch(endpoint);
  const inventory = await res.json();
  return { inventory };
}

export const actions = {
  default: async ({ request, fetch }) => {
    const data = await request.formData();
    const inventoryId = data.get("inventory-id");
    const res = await fetch("/API/Market/new", {
      method: "post",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ inventoryId })
    });
    if (!res.ok) {
      throw res;
    }
  }
} satisfies Actions;
