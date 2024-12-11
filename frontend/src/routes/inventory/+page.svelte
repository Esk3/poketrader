<script lang="ts">
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";

  const { data } = $props();

  const pokemon = data.inventory.pokemon.reduce((map, item) => {
    map[item.pokemonId] = item;
    return map;
  }, {});

  const inventory = data.inventory.items.map((item) => {
    return { item, pokemon: pokemon[item.pokemonId] };
  });
</script>

{#each inventory as { item, pokemon }}
  <div>
    <p>{pokemon.name}</p>
    <PokemonSprite url={pokemon.spriteUrl} name={pokemon.name} />
    {#if item.count}
      <p>count: {item.count}</p>
    {/if}
  </div>
{/each}
