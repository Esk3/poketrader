import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params, fetch }) => {
  const listingId = params.listingId;
  const res = await fetch("/API/Market/" + listingId + "/bids/info");
  const bids = await res.json();
  const inventoryRes = await fetch("/API/Inventory/info/grouped");
  const inventory = await inventoryRes.json();
  return {
    listingId,
    bids,
    inventory
  }
}

export const actions = {
  bid: async ({ request, params, fetch }) => {
    const data = await request.formData();
    const amount = data.get("amount");
    const res = await fetch("/API/Market/" + params.listingId + "/bid", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ amount })
    });
    if (!res.ok) {
      console.log(res);
    }
  }
} satisfies Actions;
