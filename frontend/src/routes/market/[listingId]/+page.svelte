<script>
  import ItemView from "$lib/components/ItemView.svelte";
  const { data } = $props();
  let selectedItem = $state(null);
  console.log(data);
</script>

{#each data.bids as bid}
  <form method="post" action="?/finish">
    <input type="hidden" name="winner-username" value={bid.username} />
    <input type="submit" value="Make winner" />
  </form>
  <p>{bid.username}</p>
  <p>{bid.totalValue}</p>
  {#each bid.items as fut}
    {#await fut then item}
      <ItemView {item} />
    {/await}
  {/each}
{/each}

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

{#each data.inventory as fut}
  {#await fut then item}
    <div onclick={() => (selectedItem = item)}>
      <ItemView {item} />
    </div>
  {/await}
{/each}
