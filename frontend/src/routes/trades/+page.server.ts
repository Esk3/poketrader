export const load = async ({ fetch }) => {
  const res = await fetch("/API/trades");
  const trades = await res.json();
  console.log(trades)
  return { trades };
}
