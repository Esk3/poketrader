<script lang="ts">
  import PokemonSprite from "$lib/components/PokemonSprite.svelte";
  import { onMount } from "svelte";

  const ApiUrl = new URL("http://localhost:5172");
  const { bid } = $props();

  const getPokemonItem = async (id) => {
    const res = await fetch(ApiUrl + "Inventory/item/" + id);
    const data = await res.json();
    return data;
  };
</script>

<p>User: {bid.pokemonUserId}</p>
<p>amount: {bid.amount}</p>

{#each bid.inventoryIds as id}
  {#await getPokemonItem(id) then value}
    <PokemonSprite id={value.pokemonId} />
  {/await}
{/each}
