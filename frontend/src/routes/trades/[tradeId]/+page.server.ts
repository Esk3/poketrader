import type { Actions, PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch, params }) => {
  const tradeId = params.tradeId;
  const res = await fetch("/API/Trades/" + tradeId);
  const trade = await res.json();

  const inventory1 = trade.user1ItemsUrls.map(async url => {
    const res = await fetch(url);
    const data = await res.json();
    return data;
  });

  const inventory2 = trade.user2ItemsUrls.map(async url => {
    const res = await fetch(url);
    const data = await res.json();
    return data;
  });

  if (!trade.trade.endTimestamp) {
    const inventoryRes = await fetch("/API/Inventory");
    let inventory = await inventoryRes.json();
    inventory = inventory.map(async url => {
      const res = await fetch(url);
      const item = await res.json();
      return item;
    })
    console.log(inventory);
    return { trade, inventory1, inventory2, inventory };
  } else {
    return { trade, inventory1, inventory2 };
  }
}

export const actions = {
  add: async ({ request, fetch, params }) => {
    const tradeId = params.tradeId;
    const data = await request.formData();
    const inventoryId = data.get("inventory-id");
    const res = await fetch("/API/Trades/" + tradeId + "/add", {
      method: "post",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ inventoryId })
    });
    if (!res.ok) {
      throw res;
    }
  },
  remove: async ({ request, fetch, params }) => {
    const tradeId = params.tradeId;
    const data = await request.formData();
    const inventoryId = data.get("inventory-id");
    const res = await fetch("/API/Trades/" + tradeId + "/remove", {
      method: "post",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ inventoryId })
    });
    if (!res.ok) { throw res }
  },
  lockin: async ({ fetch, params }) => {
    const tradeId = params.tradeId;
    const res = await fetch("/API/Trades/" + tradeId + "/lockin", {
      method: "post"
    });
  },
} satisfies Actions;
