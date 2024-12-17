import type { PageServerLoad } from "./$types";

export const load: PageServerLoad = async ({ fetch }) => {
  const res = await fetch("/API/trades");
  const view = await res.json();
  console.log(view)
  return { ...view };
}
