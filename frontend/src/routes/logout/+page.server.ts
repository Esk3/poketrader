import type { Actions } from "./$types";

export const actions = {
  default: async ({ request, fetch, cookies }) => {
    const res = await fetch("/API/Auth/signout", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ userName: "string" })
    });
    cookies.delete(".AspNetCore.Identity.Application", { path: "/" });
    if (!res.ok) {
      throw res;
    }
  }
} satisfies Actions;
