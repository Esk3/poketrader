<script>
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";
  import BidsList from "./BidsList.svelte";
  const { data } = $props();
  let selectedItem = $state(null);
  console.log(data.inventory);
</script>

<BidsList bids={data.bids} />
{#if selectedItem}
  <PokemonSprite id={selectedItem.pokemonId} />
  abc
{/if}
<form method="post" action="?/bid">
  {#if selectedItem}
    <input
      type="hidden"
      name="inventory-id"
      value={selectedItem.item.inventoryId}
    />
  {/if}
  <input type="number" name="amount" placeholder="amount" required />
  <input type="Submit" value="Submit" />
</form>

{#each data.inventory.items as item}
  <PokemonSprite id={item.pokemonId} onclick={() => (selectedItem = item)} />
{/each}
