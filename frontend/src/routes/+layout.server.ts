
export const load = async ({ fetch, cookies }: any) => {
  const id = cookies.get(".AspNetCore.Identity.Application");
  let user;
  if (id) {
    const res = await fetch("/API/User");
    const username = await res.text();
    user = {
      username
    };
  }
  const pages = [
    { href: "/", name: "Home" },
    { name: "Pokemon", href: "/pokemon" },
    { name: "Shop", href: "/shop" },
    { name: "Market", href: "/market" },
    { name: "Inventory", href: "/inventory" },
  ];
  return { user, pages };
}
