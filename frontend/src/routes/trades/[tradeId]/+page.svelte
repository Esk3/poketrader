<script lang="ts">
  import ItemView from "$lib/components/ItemView.svelte";
  import type { PageServerData } from "./$types";

  const { data }: { data: PageServerData } = $props();
  console.log(data);
  const finished = data.trade.trade.endTimestamp != null;
</script>

<a href="/trades">Back</a>
{#if finished}
  <h2>Trade Finished At: {data.trade.trade.endTimestamp}</h2>
{/if}

{#each data.inventory1 as fut}
  {#await fut then item}
    <ItemView {item} />
    {#if !finished}
      <form method="post" action="?/remove">
        <input type="hidden" name="inventory-id" value={item.id} />
        <input type="submit" value="Remove" />
      </form>
    {/if}
  {/await}
{/each}

<hr />

{#each data.inventory2 as fut}
  {#await fut then item}
    <ItemView {item} />
  {/await}
{/each}

{#if !finished}
  <hr />
  <form method="post" action="?/lockin">
    <input type="submit" value="lockin" />
  </form>
  <hr />

  {#each data.inventory as fut}
    {#await fut then item}
      <ItemView {item} />
      <form method="post" action="?/add">
        <input type="hidden" name="inventory-id" value={item.id} />
        <input type="submit" value="Add" />
      </form>
    {/await}
  {/each}
{/if}
