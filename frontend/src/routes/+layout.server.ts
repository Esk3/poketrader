
export const load = async ({ fetch, cookies }: any) => {
  const id = cookies.get(".AspNetCore.Identity.Application");
  let user: { username: string, pokemonUserId: string, coins: number } | undefined;
  if (id) {
    const res = await fetch("/API/User");
    user = await res.json();
  }
  const pages = [
    { href: "/", name: "Home" },
    { name: "Pokemon", href: "/pokemon" },
    { name: "Shop", href: "/shop" },
    { name: "Market", href: "/market" },
    { name: "Inventory", href: "/inventory" },
    { name: "Trades", href: "/trades" }
  ];
  return { user, pages };
}
