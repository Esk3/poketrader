<script>
  import FlipCard from "$lib/components/FlipCard.svelte";
  import ItemView from "$lib/components/ItemView.svelte";
  import PokemonCardLoading from "$lib/components/PokemonCardLoading.svelte";
  const { data } = $props();
  let selectedItem = $state(null);
  const eier = data.user.username == data.listing.username;
  console.log(data);
</script>

pris:
{#await data.item}
  <PokemonCardLoading />
{:then item}
  <FlipCard width="130px" height="200px" loading={PokemonCardLoading}>
    <ItemView {item} />
  </FlipCard>
{/await}

<hr />

{#each data.bids as bid}
  {#if eier}
    <form method="post" action="?/finish">
      <input type="hidden" name="winner-username" value={bid.username} />
      <input type="submit" value="Make winner" />
    </form>
  {/if}
  <p>{bid.username}</p>
  <p>{bid.totalValue}</p>

  {#each bid.items as fut, i}
    {#await fut}
      <PokemonCardLoading />
    {:then item}
      <FlipCard
        width="130px"
        height="200px"
        delay={(i + 5) * 100}
        loading={PokemonCardLoading}
      >
        <ItemView {item} />
      </FlipCard>
    {/await}
  {/each}
{/each}

{#if !eier}
  <hr />
  {#if selectedItem}
    <ItemView item={selectedItem} />
  {/if}
  <form method="post" action="?/bid">
    {#if selectedItem}
      <input type="hidden" name="inventory-id" value={selectedItem.id} />
    {/if}
    <input type="Submit" value="Submit" />
  </form>
  <hr />

  {#each data.inventory as fut, i}
    {#await fut}
      <PokemonCardLoading />
    {:then item}
      <FlipCard
        width="130px"
        height="200px"
        delay={(i + 5) * 100}
        loading={PokemonCardLoading}
      >
        <div onclick={() => (selectedItem = item)}>
          <ItemView {item} />
        </div>
      </FlipCard>
    {/await}
  {:else}
    ingen items i inventory
  {/each}
{/if}
