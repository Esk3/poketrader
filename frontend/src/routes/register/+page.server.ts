import { fail, redirect } from "@sveltejs/kit";
import type { Actions } from "./$types";

export const actions = {
  default: async ({ request, fetch }) => {
    const data = await request.formData();
    const userName = data.get("username");
    const res = await fetch("/API/Auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ userName })
    });
    if (!res.ok) {
      return fail(400, { err: true });
    }
    return redirect(301, "/login");
  }
} satisfies Actions;
