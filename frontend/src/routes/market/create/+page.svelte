<script lang="ts">
  import ItemView from "$lib/components/ItemView.svelte";

  const { data } = $props();

  console.log(data);
  let selected = $state(null);
</script>

<h1>Ny</h1>

{#if selected}
  <ItemView item={selected} />
{/if}
<form method="post">
  {#if selected}
    <input type="hidden" name="inventory-id" value={selected.id} />
  {/if}
  <input type="submit" />
</form>

{#each data.inventory as fut}
  {#await fut then item}
    <div onclick={() => (selected = item)}>
      <ItemView {item} />
    </div>
  {/await}
{/each}
