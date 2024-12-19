<script lang="ts">
  import "./app.css";
  import NavBar from "./NavBar.svelte";
  const { children, data }: any = $props();
  console.log(data.user);
</script>

<header>
  <NavBar pages={data.pages} user={data.user} />
</header>

<main>
  {@render children()}
</main>

<style>
header {
  width: 100%;
}
</style>
