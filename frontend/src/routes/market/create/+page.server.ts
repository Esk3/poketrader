import { ApiUrl } from "$lib";
import { redirect } from "@sveltejs/kit";
import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch, }) => {
  const res = await fetch("/API/Inventory");
  let inventory = await res.json();
  inventory = inventory.map(async url => {
    const res = await fetch(url);
    const item = await res.json();
    return item;
  });
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
    return redirect(303, "/market");
  }
} satisfies Actions;
