import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ params, fetch }) => {
  const listingId = params.listingId;
  const res = await fetch("/API/Market/" + listingId + "/bids");
  let bids = await res.json();
  console.log(bids);
  bids = bids.map(bid => {
    return {
      items: bid.itemUrls.map(async url => {
        const res = await fetch(url);
        return await res.json();
      }), ...bid
    }
  });
  const inventoryRes = await fetch("/API/Inventory");
  let inventory = await inventoryRes.json();
  inventory = inventory.map(async url => {
    const res = await fetch(url);
    return res.json();
  })
  return {
    listingId,
    bids,
    inventory
  }
}

export const actions = {
  bid: async ({ request, params, fetch }) => {
    const data = await request.formData();
    const inventoryId = data.get("inventory-id");
    const res = await fetch("/API/Market/" + params.listingId + "/bid", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ inventoryId })
    });
    if (!res.ok) {
      console.log(res);
    }
  },
  finish: async ({ request, params, fetch }) => {
    const data = await request.formData();
    const winnerUsername = data.get("winner-username");
    const res = await fetch("/API/Market/" + params.listingId + "/finish", {
      method: "post",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ winnerUsername })
    });
    if (!res.ok) {
      throw res;
    }
  }
} satisfies Actions;
