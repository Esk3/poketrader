<script>
  import FlipCard from "$lib/components/FlipCard.svelte";
  import ItemView from "$lib/components/ItemView.svelte";
  import PokemonCardLoading from "$lib/components/PokemonCardLoading.svelte";
  const { data } = $props();
  let selectedItem = $state(null);
  const eier = data.user.username == data.listing.username;
  const closed = data.listing.closedTimestamp != null;
  console.log(data);
</script>

{#if closed}
  <h1>Listing closed at: {data.listing.closedTimestamp}</h1>
{/if}

<a href="/market">Back</a>
pris:
{#await data.item}
  <PokemonCardLoading />
{:then item}
  <FlipCard width="130px" height="200px" loading={PokemonCardLoading}>
    <ItemView {item} />
  </FlipCard>
{/await}

<hr />
{#if closed}
  <h2>TODO: winner</h2>
  <hr />
{/if}

<p>Bids:</p>
<ul>
  {#each data.bids as bid}
    <li class="bid">
      <div class="info">
        {#if eier && !closed}
          <form method="post" action="?/finish">
            <input type="hidden" name="winner-username" value={bid.username} />
            <input type="submit" value="Make winner" />
          </form>
        {/if}
        <p>{bid.username}</p>
        <p class="money">
          {bid.totalValue}
        </p>
      </div>

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
    </li>
  {:else}
    <p>det er ingen bids {!eier ? "ver den første til og bidde" : ""}</p>
  {/each}
</ul>

{#if !eier && !closed}
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

<style>
  ul {
    display: flex;
    flex-direction: column;
    gap: 1em;
    list-style: none;
    padding: 0;
  }
  .bid {
    width: 100%;
    background-color: gray;
    display: flex;
    gap: 1em;
    padding: 1em;
    border-radius: 1em;
  }
</style>
