<script lang="ts">
  import type { PageData } from "./$types";
  import PokemonName from "./PokemonName.svelte";
  const { data, pokemon }: { data: PageData } = $props();
</script>

<ul>
  {#each pokemon.results as p}
    <li>
      <PokemonName pokemon={p} />
    </li>
  {/each}
</ul>
