<script lang="ts">
  import PokemonNameList from "./PokemonNameList.svelte";
  import type { PageData } from "./$types";
  import PokemonPreView from "$lib/components/PokemonPreView.svelte";

  const { data }: { data: PageData } = $props();
</script>

<h1>Pokemon</h1>

<ul>
  {#each data.pokemon as pokemon}
    <li>
      <PokemonPreView {pokemon} />
    </li>
  {/each}
</ul>

<style>
  ul {
    list-style: none;
    padding: 0;
    display: flex;
    flex-wrap: wrap;
    gap: 1em;
  }
</style>
