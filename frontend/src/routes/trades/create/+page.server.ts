import { redirect } from "@sveltejs/kit";
import type { Actions } from "./$types";

export const actions = {
  default: async ({ request, fetch }) => {
    const data = await request.formData();
    const otherUsername = data.get("other-username");
    const res = await fetch("/API/Trades/create", {
      method: "post",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ otherUsername })
    });
    if (!res.ok) {
      throw res;
    }
    return redirect(303, "/trades")
  }
} satisfies Actions;
