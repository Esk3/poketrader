<script lang="ts">
  const { data } = $props();

  const pokemon = data.inventory.pokemon.reduce((map, item) => {
    map[item.pokemonId] = item;
    return map;
  }, {});

  const inventory = data.inventory.items.map((item) => {
    return { item, pokemon: pokemon[item.pokemonId] };
  });
  console.log(inventory);
  let selected = $state(null);
</script>

<h1>Ny</h1>

{#if selected}
  <img src={selected.pokemon.spriteUrl} />
{/if}
<form method="post">
  {#if selected}
    <input
      type="hidden"
      name="inventory-id"
      value={selected.item.inventoryId}
    />
  {/if}
  <input type="submit" />
</form>

{#each inventory as { item, pokemon }}
  <img
    src={pokemon.spriteUrl}
    onclick={() => {
      selected = { item, pokemon };
    }}
  />
{/each}
