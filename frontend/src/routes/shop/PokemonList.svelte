<script lang="ts">
  import Pokemon from "./Pokemon.svelte";

  const { pokemon } = $props();
  console.log(pokemon);
</script>

<ul>
  {#each pokemon as p}
    <li>
      <a href="/shop/{p.pokemonId}">
        <Pokemon pokemon={p} />
      </a>
    </li>
  {/each}
</ul>
