import { fail, redirect } from "@sveltejs/kit";
import type { Actions } from "./$types";
import type { PageLoad } from "../pokemon/$types";

export const actions = {
  default: async ({ request, fetch, cookies }) => {
    const data = await request.formData();
    const userName = data.get("username");
    const res = await fetch("/API/Auth/signin", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ userName })
    });
    if (res.status == 400) {
      return fail(400, { err: true });
    }
    if (!res.ok) {
      throw res;
    }
    const [name, value] = res.headers.getSetCookie()[0].split("=");
    cookies.set(name, value.split(";")[0], { path: "/" });
    redirect(301, "/");
  }
} satisfies Actions;
